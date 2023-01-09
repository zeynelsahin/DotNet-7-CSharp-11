// See https://aka.ms/new-console-template for more information

//Raw String Literals
/* Satırlar en son tırnak işaretinin tabıya aynı konumda veya daha ileride başlayabilir ve son tırnagın önündeki tablar boş geçilir. */

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Tar;
using System.Security.AccessControl;
using System.Text;

string message = """        
    lOR
    Try Case 
    """;
Console.WriteLine("Literal string");
Console.WriteLine(message);

// Raw String Literals with String Interpolation
var longitude = "8.808691";
var latitude = "41.158085";
var location = $$""" 
    Gps coordinates : {{{longitude}} , {{latitude}}}
""";

Console.Write(location);
var nameList = new Dictionary<string, string>()
{
    { "Zeynel", "Şahin" },
    { "İsim", "Şahin" },
    { "Ad", "Şahin" }
};

var json = $$""" 
    [
     {
        "isim":"Zeynel",
        "soyIsim":{{nameList["Zeynel"]}}
     },
     {
        "isim":"İsim",
        "soyIsim":{{nameList["İsim"]}}
     },
     {
        "isim":"Ad",
        "soyIsim":{{nameList["Ad"]}}
     }
    ]

    """;

Console.Write(json);

//Utf 8 string literals

ReadOnlySpan<byte> u8 = "Hello world in UTF-8"u8;
Console.WriteLine(Encoding.UTF8.GetString(u8));

// The Required Modifier
var person = new Person
{
    FirstName = "" //person required olarak süslendi initialize edilmesi gerekmektedir.
};

var employee = new Employee("Deneme");

//Auto Default Struct
var example = new Example();
Console.Write(example);

// File scoped Yanlızca yazılan dosyada geçerli olacaktır

// var scoped = new LocalClass(); LocalClass sınıf file modiferına sahip olduğu için buradan erişemiyoruz


//List Patterns

int[] numbers = new[] { 1, 2, 6, 7, 9 };

bool result;

result = numbers is [1, 2, 6, 7, 9]; //true
result = numbers is [1, 6, 7, 9]; //false
result = numbers is [1, 5, 7, 9]; //false

//discard 

result = numbers is [_, _, 6, _, _] İ; //true
result = numbers is [_, _, 6, ..]; //true

//range
result = numbers is [1, 2, .., 9]; //true

//var 

result = numbers is [.., var a, var b, _, _];


//Date Changed Microseconds Nanoseconds
var stopWatch = Stopwatch.StartNew();

stopWatch.Stop();
Console.WriteLine("Total Milliseconds" + stopWatch.Elapsed.TotalMilliseconds);
Console.WriteLine("Total Microseconds" + stopWatch.Elapsed.Microseconds);
Console.WriteLine("Total NanoSeconds" + stopWatch.Elapsed.TotalNanoseconds);

var dateExample = DateTime.Now.AddMicroseconds(100);

//TarFile
// TarFile.CreateFromDirectory(sourceDirectoryName:@"C:\TarDeneme",destinationFileName:@"C:\TarDeneme",includeBaseDirectory:true);
var targetTarFile = @"C:\TarDeneme2\deneme.tar";
if (File.Exists(targetTarFile))
{
    File.Delete(targetTarFile);
}
TarFile.CreateFromDirectory(@"C:\TarDeneme",targetTarFile,false);
var destination = @$"C:\TarDeneme\{Guid.NewGuid()}";
Directory.CreateDirectory(destination);
TarFile.ExtractToDirectory(targetTarFile,destination,true);
//Type Converters
TypeConverter dateOnlyConverter = TypeDescriptor.GetConverter(typeof(DateOnly));
DateOnly? dateOnly = dateOnlyConverter.ConvertFromString("2023-01-10") as DateOnly?;


/* .Net 7 New Features

HTTP/2 WebSockets
HTTP/3 
Output caching
Rate limiting
Request decompression
*/
Console.ReadKey();

class Example
{
    private string Deneme { get; set; }
    private bool IsValid { get; set; } //default değeri : false
    private DateTime TrnDate { get; set; } //default değeri  1.01.0001 00:00:00

    public override string ToString()
    {
        return $"Deneme: {Deneme} IsValid : {IsValid} TrnDate : {TrnDate}";
    }
}


class Person
{
    public required string FirstName { get; set; }
}

class Employee : Person
{
    [SetsRequiredMembers] // constructor dışındaki required alanların yok sayar
    public Employee(string function)
    {
        Function = function;
    }

    public required string Function { get; set; }
}