using Multithreading;
using Multithreading.Reports;
using System.Collections.Concurrent;
using System.Text.Json;

var reportsQueue = new ConcurrentQueue<IReport>();

int checkIntervalMs = 5000;

string directoryPath = "C:\\Users\\Tsuni\\Desktop\\TestFolder";

IEnumerable<FileContent> GetDirectoryFileContents(string directoryPath)
{
    foreach (string filename in Directory.GetFiles(directoryPath))
    {
        yield return new FileContent
        {
            Name = filename,
            CheckSum = FileHasher.GetCheckSum(filename)
        };
    }
}

void CheckFilesContent(IEnumerable<FileContent> oldFileContents, IEnumerable<FileContent> newFileContents)
{
    int oldFileContentsCount = oldFileContents.Count();
    int newFileContentsCount = newFileContents.Count();

    if (oldFileContentsCount < newFileContentsCount)
    {

        var difference = newFileContents.Where(newFile => !oldFileContents.Any(oldFile => newFile.Name == oldFile.Name));


        foreach (var file in difference)
        {
            var report = new FileCreateReport
            {
                FileName = file.ShortName,
                CheckSum = file.CheckSum
            };
            reportsQueue.Enqueue(report);
        }
        return;

    }
    if (oldFileContentsCount > newFileContentsCount)
    {

        var difference = oldFileContents.Where(oldFile => !newFileContents.Any(newFile => oldFile.Name == newFile.Name));


        foreach (var file in difference)
        {
            var report = new FileRemoveReport
            {
                FileName = file.ShortName,
                CheckSum = file.CheckSum
            };
            reportsQueue.Enqueue(report);
        }
        return;

    }




    for (int i = 0; i < oldFileContentsCount; i++)
    {
        if (oldFileContents.ElementAt(i).CheckSum != newFileContents.ElementAt(i).CheckSum)
        {
            var report = new FileUpdateReport
            {

                FileName = oldFileContents.ElementAt(i).ShortName,
                OldCheckSum = oldFileContents.ElementAt(i).CheckSum,
                NewCheckSum = newFileContents.ElementAt(i).CheckSum
            };
            reportsQueue.Enqueue(report);
        }
    }
}
///////////////////////////////////////////////////////

Thread fileCheckingThread = new Thread((directoryPath) =>
{
    var actualFileContents = GetDirectoryFileContents(directoryPath.ToString()).ToList();

    while(true)
    {
        Thread.Sleep(checkIntervalMs);

        var newFileContents = GetDirectoryFileContents(directoryPath.ToString()).ToList();

        CheckFilesContent(actualFileContents, newFileContents);

        actualFileContents.Clear();
        actualFileContents.AddRange(newFileContents);
    }

});

fileCheckingThread.Start(directoryPath);

////////////////////////////////////////////////

Thread queueReadingThread = new Thread((directoryPath) =>
{

    while (true)
    {
        IReport incomeReport;
        bool success = reportsQueue.TryDequeue(out incomeReport);

        if (success)
        {
            switch (incomeReport)
            {
                case FileUpdateReport fileUpdateReport:
                    Console.WriteLine("File updated: " + JsonSerializer.Serialize(fileUpdateReport));
                    break;

                case FileCreateReport fileCreateReport:
                    Console.WriteLine("File created: " + JsonSerializer.Serialize(fileCreateReport));
                    break;
                case FileRemoveReport fileCreateReport:
                    Console.WriteLine("File removed: " + JsonSerializer.Serialize(fileCreateReport));
                    break;

                default:
                    throw new Exception("Unknown report");
            }
        }

        Thread.Sleep(100);
    }
});

queueReadingThread.Start();


queueReadingThread.Join();