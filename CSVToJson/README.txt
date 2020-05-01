---------------------------------------------------------------------------------------
-- CSVToJson
-- Simple CSV file to Json converter

-- This code implements multiple input and object creation options. It is intended to demonstrate
-- OO principles and design.

Input types:
------------
	NaiveCSVFile			A very basic CSV reader, just uses string.Split on each line
	NugetCSVFile			A CSV reader based on a nuget package. This is the default.
	SqlServerDataSource		Demonstrates how the functionality can take in data from a non-csv input

Object creation options:
------------------------
	Dynamic					Uses ExpandoObject to read the data in. This is the default.
	StronglyTyped			Creates Person objects from the data. The Person/Address classes are in the Sample
							namespace, demonstrates basic mappings using a simple syntax that should be familiar if 
							you've used Fluent NHibernate

Run the exe without parameters to see the help text. NB: In this example code the XmlOutput routine is not coded and will throw a
NotImplementedException.

Sample usage:
-------------

csvtojson -i sample.csv
							Reads in sample.csv, uses the NugetCSVFile input type and the dynamic creation option.

csvtojson -c StronglyTyped -t NaiveCSVFile -i sample.csv
							Reads in sample.csv, uses the NaiveCSVFile input type and the strongly typed creation option.
							NB: The property names on the strongly type objects are in PROPER case so the json output is slightly
							different from the dynamic object creation option.

csvtojson -t SqlServerDataSource -i "Server=ANDY-DELL\SQL2019DEV;Initial Catalog=JsonTest;Integrated Security=SSPI;"
							Demonstrates dynamic object creation, reading data from a SQL server datasource. There are database scripts
							int the SampleData folder that creates 2 tables and a view, and the datasource is hard coded to do a select * 
							from the view. 

Other stuff:
----------------------
Both object creation modes should (theoretically!) allow for an object hierarchy, although this has only been tested with the dynamic 
object creator. The file SampleData\another-sample.csv has content:

name,properties_colour,properties_size_width,properties_size_height
Ball,Red,10,10

which when processed with

csvtojson -i another-sample.csv

returns
[
  {
    "name": "Ball",
    "properties": {
      "colour": "Red",
      "size": {
        "width": "10",
        "height": "10"
      }
    }
  }
]




Build notes
----------------------

This solution was developed in Visual Studio Community 2019. 

Nuget packages used:

CommandLineParser		- For the command line parsing, obviously :)
https://github.com/commandlineparser/commandline

Csv						- For the NugetCSVFile input type, it's a bit buggy but it demonstrates the principle
https://github.com/stevehansen/csv/

Newtonsoft.Json			- For the json output
https://www.newtonsoft.com/json
