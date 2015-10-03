using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace MongoSample
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/test");
#pragma warning disable CS0618 // Type or member is obsolete
            MongoServer server = client.GetServer();
#pragma warning restore CS0618 // Type or member is obsolete

            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> bankdata = database.GetCollection<BsonDocument>("bank_data");

            BsonDocument person = new BsonDocument
            {
                {"first_name","Steven" },
                { "last_name","Edouard"},
                {"accounts", new BsonArray
                {
                    new BsonDocument
                    {
                        {"account_balance",50000000 },
                        {"account_type","Investment" },
                        {"currency","USD" }
                    }
                } }
            };

            bankdata.Insert(person);
            System.Console.WriteLine(person["_id"]);

            person["accounts"][0]["account_balanace"] = person["accounts"][0]["account_balance"].AsInt32 + 100000;

            bankdata.Save(person);
            System.Console.WriteLine("Successfully updated 1 document");

            BsonDocument newPerson = bankdata.FindOneById(person["_id"]);

            System.Console.WriteLine(newPerson["accounts"][0]["account_balance"].AsInt32);

            var query = Query.EQ("_id", newPerson["_id"]);
            WriteConcernResult result = bankdata.Remove(query);
            System.Console.WriteLine("number of documents removed " + result.DocumentsAffected);
        }
    }
}
