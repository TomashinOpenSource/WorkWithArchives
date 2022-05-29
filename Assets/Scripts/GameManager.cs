using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO.Compression;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private string archivesPath;
    [SerializeField] private string replaceableFilesPath;
    [SerializeField] private TMP_Text debugTextField;

    [Header("State")]
    [SerializeField] private string[] filesInArchivesPath;
    [SerializeField] private string[] filesInReplaceableFilesPath;

    public string ArchivesPath { get => archivesPath; set => archivesPath = value; }
    public string ReplaceableFilesPath { get => replaceableFilesPath; set => replaceableFilesPath = value; }

    private void Start()
    {
        /*
        filesInPath = Directory.GetFiles(ArchivesPath);
        
        using (ZipArchive zip = ZipFile.Open(filesInPath[0], ZipArchiveMode.Read))
            foreach (ZipArchiveEntry entry in zip.Entries)
                Debug.Log(entry.Name);
        
        using (FileStream zipToOpen = new FileStream(filesInPath[0], FileMode.Open))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                ZipArchiveEntry readmeEntry = archive.CreateEntry("Readme.txt");
                using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                {
                    writer.WriteLine("Information about this package.");
                    writer.WriteLine("========================");
                }
            }
        }
        */
    }

    public void OnButtonPressed()
    {
        if (string.IsNullOrEmpty(ArchivesPath) || string.IsNullOrEmpty(ReplaceableFilesPath))
        {
            DebugLog(string.Format( "ArchivesPath.IsNullOrEmpty = {0}, " +
                                    "ReplaceableFilesPath.IsNullOrEmpty = {1}", 
                                    string.IsNullOrEmpty(ArchivesPath), 
                                    string.IsNullOrEmpty(ReplaceableFilesPath)));
            return;
        }

        filesInArchivesPath = Directory.GetFiles(ArchivesPath);
        filesInReplaceableFilesPath = Directory.GetFiles(ReplaceableFilesPath);

        StartCoroutine(Changing());
    }

    private IEnumerator Changing()
    {
        foreach (var archivePath in filesInArchivesPath)
        {
            //string name = GetFileName(archivePath, true);
            string name = Path.GetFileNameWithoutExtension(archivePath);
            string fileName = string.Format("BPAT22.{0}.0.HR.S01", name);
            string fileNamePath = filesInReplaceableFilesPath.ToList().Find(path => Path.GetFileName(path) == fileName);
            ZipArchive zipArchive = ZipFile.Open(archivePath, ZipArchiveMode.Update);
            ZipArchiveEntry oldZipArchiveEntry = zipArchive.Entries.ToList().Find(entry => entry.Name == fileName);
            ZipArchiveEntry newZipArchiveEntry = zipArchive.CreateEntryFromFile(fileNamePath, oldZipArchiveEntry.FullName);
            oldZipArchiveEntry.Delete();
            DebugLog(string.Format("{0} -> {1} -> {2} -> {3}", archivePath, name, fileName, newZipArchiveEntry.FullName));
            zipArchive.Dispose();
            yield return null;
        }
    }

    private void DebugLog(string text)
    {
        Debug.Log(text);
        debugTextField.text = text;
    }
}
