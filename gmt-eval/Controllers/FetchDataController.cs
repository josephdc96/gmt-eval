using System;
using System.Threading.Tasks;
using gmt_eval.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace gmt_eval.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FetchDataController : Controller
    {
        [HttpGet("FetchInternetData")]
        public async Task<InternetData> FetchInternetData()
        {
            try
            {
                var client =
                    new MongoClient(
                        "mongodb+srv://jcauble:cauJoseph96@joseph-dhgna.gcp.mongodb.net/test?retryWrites=true");
                var database = client.GetDatabase("gmt-eval");
                var collection = database.GetCollection<BsonDocument>("data");

                double numNet = 0;
                double sumNet = 0;
                double numNo = 0;
                double sumNo = 0;

                InternetData dat = new InternetData
                {
                    WithNet = -1,
                    WithoutNet = -1
                };

                await collection.Find(new BsonDocument()).ForEachAsync(x =>
                {
                    if (x["internet"].AsBoolean)
                    {
                        numNet++;
                        sumNet += x["G3"].AsInt32;
                    }
                    else
                    {
                        numNo++;
                        sumNo += x["G3"].AsInt32;
                    }
                });

                if (numNet > 0) dat.WithNet = sumNet / numNet;
                if (numNo > 0) dat.WithoutNet = sumNo / numNo;

                return dat;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return null;
            }
        }
        
        [HttpGet("FetchFailureData")]

        public async Task<double[]> FetchFailureData()
        {
            try
            {
                var client =
                    new MongoClient(
                        "mongodb+srv://jcauble:cauJoseph96@joseph-dhgna.gcp.mongodb.net/test?retryWrites=true");
                var database = client.GetDatabase("gmt-eval");
                var collection = database.GetCollection<BsonDocument>("data");

                double[] vals = new double[4];
                double[] totals = new double[4];

                await collection.Find(new BsonDocument()).ForEachAsync(x =>
                {
                    int y = x["failures"].AsInt32;
                    vals[y] += x["G3"].AsInt32;
                    totals[y]++;
                });

                for (int i = 0; i < 4; i++)
                {
                    if (totals[i] > 0) vals[i] = vals[i] / totals[i];
                    else vals[i] = -1;
                }

                return vals;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return null;
            }
        }
    }
}