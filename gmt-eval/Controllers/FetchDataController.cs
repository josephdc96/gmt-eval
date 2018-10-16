using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gmt_eval.Models;
using Microsoft.AspNetCore.Internal;
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
        
        [HttpGet("FetchStudyTimeData")]
        public async Task<double[]> FetchStudyTimeData()
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
                    int y = x["studytime"].AsInt32;
                    vals[y - 1] += x["G3"].AsInt32;
                    totals[y - 1]++;
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

        [HttpGet("FetchAbsenceData")]
        public async Task<TrendlineData> FetchAbsenceData()
        {
            try
            {
                var client =
                    new MongoClient(
                        "mongodb+srv://jcauble:cauJoseph96@joseph-dhgna.gcp.mongodb.net/test?retryWrites=true");
                var database = client.GetDatabase("gmt-eval");
                var collection = database.GetCollection<BsonDocument>("data");

                double[] vals = new double[94];
                double[] totals = new double[94];

                await collection.Find(new BsonDocument()).ForEachAsync(x =>
                {
                    int y = x["absences"].AsInt32;
                    vals[y] += x["G3"].AsInt32;
                    totals[y]++;
                });

                List<XYAxes> result = new List<XYAxes>();
                for (int i = 0; i < 94; i++)
                {
                    if (totals[i] > 0)
                        result.Add(new XYAxes
                        {
                            x = i,
                            y = vals[i] / totals[i]
                        });
                }

                return new TrendlineData
                {
                    data = result,
                    trendData = CalculateTrendline(result)
                };
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("FetchHealthData")]
        public async Task<HealthData> FetchHealthData()
        {
            var client =
                new MongoClient(
                    "mongodb+srv://jcauble:cauJoseph96@joseph-dhgna.gcp.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("gmt-eval");
            var collection = database.GetCollection<BsonDocument>("data");

            double[] g1 = new double[5];
            double[] g2 = new double[5];
            double[] g3 = new double[5];
            double[] totals = new double[5];

            await collection.Find(new BsonDocument()).ForEachAsync(x =>
            { 
                int y = x["health"].AsInt32 - 1;
                g1[y] += x["G1"].AsInt32;
                g2[y] += x["G2"].AsInt32;
                g3[y] += x["G3"].AsInt32;
                totals[y]++;
            });

            for (int i = 0; i < 5; i++)
            {
                if (totals[i] > 0)
                {
                    g1[i] = g1[i] / totals[i];
                    g2[i] = g2[i] / totals[i];
                    g3[i] = g3[i] / totals[i];
                }
                else
                {
                    g1[i] = g2[i] = g3[i] = 0;
                }
            }

            return new HealthData
            {
                g1Data = g1,
                g2Data = g2,
                g3Data = g3
            };
        }

        [HttpGet("FetchTransitData")]
        public async Task<double[]> FetchTransitData()
        {
            var client =
                new MongoClient(
                    "mongodb+srv://jcauble:cauJoseph96@joseph-dhgna.gcp.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("gmt-eval");
            var collection = database.GetCollection<BsonDocument>("data");

            double[] totals = new double[4];
            double[] result = new double[4];

            await collection.Find(new BsonDocument()).ForEachAsync(x =>
            {
                var y = x["traveltime"].AsInt32 - 1;
                result[y] += x["G3"].AsInt32;
                totals[y]++;
            });

            for (int i = 0; i < 4; i++)
            {
                if (totals[i] > 0) result[i] = result[i] / totals[i];
                else result[i] = 0;
            }

            return result;
        }

        private List<XYAxes> CalculateTrendline(List<XYAxes> data)
        {
            double sumXY = 0;
            double sumX = 0;
            double sumY = 0;
            double sumX2 = 0;
            double n = data.Count;
            data.ForEach(dat =>
            {
                sumXY += (dat.x * dat.y);
                sumX += dat.x;
                sumY += dat.y;
                sumX2 += Math.Pow(dat.x, 2);
            });

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
            double offset = (sumY - slope * sumX) / n;

            return new List<XYAxes>()
            {
                new XYAxes
                {
                    x = 0,
                    y = slope * 0 + offset
                },
                new XYAxes
                {
                    x = data[data.Count - 1].x,
                    y = slope * data[data.Count - 1].x + offset
                }
            };
        }
    }
}