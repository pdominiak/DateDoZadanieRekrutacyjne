using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp.Models;

namespace ConsoleApp
{
    public static class DataReader
    {
        public static void ImportAndPrintData(string fileToImport)
        {
            var ImportedObjects = new List<ImportedObject>() { }; //przeniesienie do zmiennej lokalnej
            var streamReader = new StreamReader(fileToImport);

            var importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                importedLines.Add(line);
            }

            foreach (var importedLine in importedLines)
            {
                var values = importedLine.Split(';');
                if (values.Length < 7)
                {
                    continue;
                }
                var importedObject = new ImportedObject();
                importedObject.Type = values[0].Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                importedObject.Name = values[1].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.Schema = values[2].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentName = values[3].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentType = values[4].Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.DataType = values[5];
                importedObject.IsNullable = values[6] == "1";
                ((List<ImportedObject>)ImportedObjects).Add(importedObject);
            }

            // assign number of children
            foreach (var importedObject in ImportedObjects)
            {
                importedObject.NumberOfChildren = ImportedObjects.Count(x => x.ParentType == importedObject.Type && x.ParentName == importedObject.Name);
            }
            PrintParentsWithChildren(ImportedObjects);
            Console.ReadKey();
        }

        private static void PrintParentsWithChildren(List<ImportedObject> ImportedObjects)
        {
            foreach (var database in ImportedObjects)
            {
                if (database.Type != Variables.DataBaseString)
                {
                    continue;
                }

                Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                // print all database's tables
                foreach (var table in ImportedObjects)
                {
                    if (table.ParentType.ToUpper() == database.Type && table.ParentName != null && table.ParentName == database.Name)
                    {
                        Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");
                        // print all table's columns
                        foreach (var column in ImportedObjects)
                        {
                            if (column.ParentType.ToUpper() == table.Type && column.ParentName == table.Name)
                            {
                                Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable ? "accepts nulls" : "with no nulls")}");
                            }
                        }
                    }
                }
            }
        }
    }
}
