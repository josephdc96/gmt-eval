using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using gmt_eval.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace gmt_eval.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDataController : Controller
    {

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var client = new MongoClient();
                var database = client.GetDatabase("gmt-eval");
                var collection = database.GetCollection<BsonDocument>("data");

                var result = string.Empty;
                using (var reader = new StreamReader(Request.Form.Files[0].OpenReadStream()))
                {
                    result = reader.ReadToEnd();
                }

                string[] rows = result.Split('\n');
                List<BsonDocument> dat = new List<BsonDocument>();

                string[] headers = rows[0].Split(';');
                for (int i = 1; i < rows.Length; i++)
                {
                    string row = new string((from c in rows[i] where c != '\"' select c).ToArray());
                    string[] cols = row.Split(';');
                    if (cols.Length < 33)
                    {
                        continue;
                    }
    
                    var doc = new BsonDocument
                    {
                        { headers[0], cols[0] == "GP" },
                        { headers[1], cols[1] == "F" },
                        { headers[2], Int32.Parse(cols[2])},
                        { headers[3], cols[3] == "U" },
                        { headers[4], cols[4] == "LE3" },
                        { headers[5], cols[5] == "T" },
                        { headers[6], Int32.Parse(cols[6]) },
                        { headers[7], Int32.Parse(cols[7]) },
                        { headers[8], cols[8] },
                        { headers[9], cols[9] },
                        { headers[10], cols[10] },
                        { headers[11], cols[11] },
                        { headers[12], Int32.Parse(cols[12]) },
                        { headers[13], Int32.Parse(cols[13]) },
                        { headers[14], Int32.Parse(cols[14]) },
                        { headers[15], cols[15] == "yes" },
                        { headers[16], cols[16] == "yes" },
                        { headers[17], cols[17] == "yes" },
                        { headers[18], cols[18] == "yes" },
                        { headers[19], cols[19] == "yes" },
                        { headers[20], cols[20] == "yes" },
                        { headers[21], cols[21] == "yes" },
                        { headers[22], cols[22] == "yes" },
                        { headers[23], Int32.Parse(cols[23]) },
                        { headers[24], Int32.Parse(cols[24]) },
                        { headers[25], Int32.Parse(cols[25]) },
                        { headers[26], Int32.Parse(cols[26]) },
                        { headers[27], Int32.Parse(cols[27]) },
                        { headers[28], Int32.Parse(cols[28]) },
                        { headers[29], Int32.Parse(cols[29]) },
                        { headers[30], Int32.Parse(cols[30]) },
                        { headers[31], Int32.Parse(cols[31]) },
                        { headers[32], Int32.Parse(cols[32]) },
    
                    };
                    dat.Add(doc);
                }    

                collection.InsertMany(dat);
                return Json("success");
            }
            catch (Exception)
            {
                return Json("fail");
            }
        }
    }
}