using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition.Face
{
    class StorageAccountHelper
    {
        public static void CreateTable()
        {

            string connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("FaceRecognitionAgeGenderAndEmotions");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }
        public static void AddToTable(Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            string connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table client.

            CloudTable table = tableClient.GetTableReference("FaceRecognitionAgeGenderAndEmotions");
            FaceRecognitionDataEntity faceRecognitionDataEntity = new FaceRecognitionDataEntity(face);
            TableOperation insertOperation = TableOperation.Insert(faceRecognitionDataEntity);

            // Execute the insert operation.
            table.Execute(insertOperation);
            Console.WriteLine("Added To Table");
        }
    }
}
