using Newtonsoft.Json;
using Consimple.Models;

//get raw json from get call
string rawJson;
using (var httpClient = new HttpClient())
    rawJson = await httpClient.GetStringAsync("http://tester.consimple.pro");
//deserialize responce
var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawJson);
var rawProducts = values?["Products"];
var rawCategories = values?["Categories"];

if(rawProducts != null && rawCategories!= null)
{
    var productsList = JsonConvert.DeserializeObject<List<Product>>(rawProducts.ToString());
    var categoriesList = JsonConvert.DeserializeObject<List<Category>>(rawCategories.ToString());
    //join tables
    var resultTable = productsList.Join(categoriesList, 
                                    p => p.CategoryId,
                                    c => c.Id,
                                    (p, c) => new 
                                    {
                                        Name = p.Name,
                                        Category = c.Name
                                    })
                                    .ToList();
    //print table
    string headers = String.Format("|{0,20}|{1,20}|", "Product name", "Category name");
    Console.WriteLine(headers);
    Console.WriteLine("|-----------------------------------------|");
    foreach (var product in resultTable) 
    {
        string row = String.Format("|{0,20}|{1,20}|", product.Name,product.Category);
        Console.WriteLine(row);
    }

}
else
    Console.WriteLine("Products or Categories List empty");


