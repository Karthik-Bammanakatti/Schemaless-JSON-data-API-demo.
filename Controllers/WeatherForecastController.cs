using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace DynamicJsonAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DynamicJsonController : ControllerBase
{
    private readonly ILogger<DynamicJsonController> _logger;

    public DynamicJsonController(ILogger<DynamicJsonController> logger)
    {
        _logger = logger;
    }

    //This endpoint is created to demonstrate how to send any kind of JSON response using "JsonResult" class to a GET request.
    //Send a get request to DynamicJson/DummyData
    [Route("DummyData")]
    public IActionResult DummyData()
    {
        JsonResult data = new JsonResult(new
        {
            Name = "Karthik",
            Age = 24,
            Hobbies = new List<string> { "Swimming", "Bowling" },
            Email = "kr70007@test.com",
            Address = new
            {
                State = "Maharashtra",
                City = "Panvel",
                Pin = "410210"
            }
        });
        return data;
    }

    //This endpoint is created to demonstrate how to send any kind of JSON response using "JObject" class to a GET request.
    //Send a get request to DynamicJson/DummyDataObject
    [Route("DummyDataObject")]
    public Object DummyDataObject()
    {
        Object person = new
        {
            Name = "Karthik",
            Age = 24,
            Hobbies = new List<string> { "Swimming", "Bowling" },
            Email = "kr70007@test.com",
            Address = new
            {
                State = "Maharashtra",
                City = "Panvel",
                Pin = "410210"
            }
        };
        return person;
    }

    //This endpoint is created to demonstrate how to accept any type of JSON data via POST request and extract its properties without creating any class.
    //Send a POST request to DynamicJson/DummyData and include any JSON object in the body.
    [HttpPost]
    [Route("DummyDataInput")]
    public Object DummyDataInput([FromBody] Object data)//We are accepting Object type from body of the request (Object type can be use to represent any type of JSON data)
    {
        //Create an object of person (Later we are going to mapp the properties of this object)
        Person? person = new Person();

        //Convert to string and deserialize the data received from the body of the request and store it in dynamic type since we dont have any class for the data received from the body of the request.
        dynamic? personData = JsonConvert.DeserializeObject<dynamic>(data.ToString());

        //Mapping of the fields.
        person.Name = personData?.Name;
        person.Age = personData?.Age;

        //Hobbies is a list of strings so we need to convert it to string deserialize it again with generic type List<string>
        person.Hobbies = JsonConvert.DeserializeObject<List<string>>(personData?.Hobbies.ToString());

        System.Console.WriteLine();
        System.Console.WriteLine();
        //Printing the JSON data we receive from the body of the request
        System.Console.WriteLine(data);

        System.Console.WriteLine();
        //Logging the person data after mapping.
        System.Console.WriteLine(Person.PersonDetails(person));

        Object response = new { status = 200, message = "JSON object received and mapped successfully." };
        return response;
    }

    //Sample 'Person' class for mapping purposes. 
    //We will map the object of this class from the dynamic data we receive via POST request.
    class Person
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public List<string>? Hobbies { get; set; }

        //This method is responsible for printing the person details to the console.
        public static string PersonDetails(Person person)
        {
            string hobstr = "[";
            for (int i = 0; i < person?.Hobbies?.Count; i++)
            {
                if (i != person.Hobbies.Count - 1)
                {
                    hobstr += person.Hobbies[i] + ", ";
                }
                else
                {
                    hobstr += person.Hobbies[i];
                }
            }
            hobstr += "]";

            return $" Name : {person?.Name},\n Age : {person?.Age},\n Hobbies : {hobstr} \n";
        }
    }
}
