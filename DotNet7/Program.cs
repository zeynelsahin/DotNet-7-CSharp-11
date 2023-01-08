// See https://aka.ms/new-console-template for more information

//Raw String Literals

string message = """
    Try Case 
Try Case 
            Try Case 
Try Case 
""";
Console.WriteLine(message);

// Raw String Literals with String Interpolation
var longitude = "8.808691";
var latitude = "41.158085";
var location = $$""" 
    Gps coordinates : {{{longitude}} , {{latitude}}}
""";
Console.Write(location);

// The Required Modifier

var person = new Person
{
    FirstName = ""
};

Console.ReadKey();


class Person
{
    public required string FirstName { get; set; }
}