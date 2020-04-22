using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceInfoController : ControllerBase
    {

        private IConfiguration _config;
        private string DBCnstr = "";
        public PlaceInfoController(IConfiguration config)
        {
            _config = config;
            //初始化DB連線字串
            DBCnstr = _config.GetValue<string>("ConnStr:RemoteDB");
        }

        /// <summary>
        /// 取得所有資訊，但可以輸入過濾條件，包含Booking的人數
        /// </summary>
        /// <returns></returns>
        // GET: api/PlaceInfo
        [HttpGet]
        public string GetPlaceInfoDetail()
        {
            DataTable dt = new DataTable("MyTable");
            string sql = "SELECT " +
                         "      a.*, " +
                         "     (SELECT SUM(IFNULL(PeopleCount, 0)) FROM SYS_BOOKINFO WHERE PlaceID = a.ID) NowCount" +
                         "  FROM" +
                         "     `SYS_PLACEINFO` a" +
                         " WHERE 1 = 1" +
                         "   AND FromDate > @now";

            try {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@now", DateTime.Now));

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);

                    cmd.Clone();
                    cmd.Dispose();
                }
            }
            catch ( Exception ex )
            {
                //發信
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }
            
           
            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 取得某人的所有開團資訊(最久看到一個月前)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/PlaceInfo/5
        [HttpGet("CreatorID/{id}")]
        public string GetPlaceInfoByCreatorID(string id)
        {
            DataTable dt = new DataTable("MyTable");
            string sql = "SELECT * FROM SYS_PLACEINFO where 1=1 " +
                         " And CreatorID = @CreatorID" +
                         " And FromDate >= @now";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@CreatorID", id));
                    cmd.Parameters.Add(new MySqlParameter("@now", DateTime.Now.AddMonths(-1)));  //最多蒐尋到1個月前的場地資訊

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);

                    cmd.Clone();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //發信
            }

            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 取得某場地的詳細資訊
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        // GET: api/PlaceInfo/5
        [HttpGet("{id}")]
        public string GetPlaceInfoByID(string ID)
        {
            DataTable dt = new DataTable("MyTable");
            string sql = "SELECT * FROM SYS_PLACEINFO where 1=1 " +
                         " And ID = @ID";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@ID", ID));
                    

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);

                    cmd.Clone();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //發信
            }

            return JsonConvert.SerializeObject(dt);
        }

        /// <summary>
        /// 新增一筆場地資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/PlaceInfo
        [HttpPost]
        public bool Post([FromBody] SYS_PLACEINFO value)
        {
            //驗證輸入結構是否符合
            if (!ModelState.IsValid) return false;

            bool _result = false;
            DataTable dt = new DataTable("MyTable");
            string sql = " Insert Into `SYS_PLACEINFO`  " +
                        "             (`Id`,  " +
                        "              `ClubName` , "+
                        "              `Placename`,  " +
                        "              `Placedesc`,  " +
                        "              `Fromdate`,  " +
                        "              `Todate`,  " +
                        "              `Max`,  " +
                        "              `Accpet`,  " +
                        "              `Level`,  " +
                        "              `Creator`,  " +
                        "              `Creatorid`,  " +
                        "              `Updatedate`)  " +
                        " Values      (@Id,  " +
                        "              @ClubName , "+
                        "              @Placename, " +
                        "              @Placedesc, " +
                        "              @Fromdate,  " +
                        "              @Todate,  " +
                        "              @Max,  " +
                        "              @Accpet,  " +
                        "              @Level,  " +
                        "              @Creator,  " +
                        "              @Creatorid,  " +
                        "              @Updatedate) ";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Id", Guid.NewGuid().ToString()));
                    cmd.Parameters.Add(new MySqlParameter("@ClubName", value.ClubName));
                    cmd.Parameters.Add(new MySqlParameter("@Placename", value.PlaceName));
                    cmd.Parameters.Add(new MySqlParameter("@Placedesc", value.PlaceDesc));
                    cmd.Parameters.Add(new MySqlParameter("@Fromdate", value.FromDate));
                    cmd.Parameters.Add(new MySqlParameter("@Todate", value.ToDate));
                    cmd.Parameters.Add(new MySqlParameter("@Max", value.Max));
                    cmd.Parameters.Add(new MySqlParameter("@Accpet", "Y"));
                    cmd.Parameters.Add(new MySqlParameter("@Level", value.Level));
                    cmd.Parameters.Add(new MySqlParameter("@Creator", value.Creator));
                    cmd.Parameters.Add(new MySqlParameter("@Creatorid", value.CreatorID));
                    cmd.Parameters.Add(new MySqlParameter("@Updatedate", DateTime.Now));

                    int effectRow = cmd.ExecuteNonQuery();

                    cmd.Clone();
                    cmd.Dispose();

                    if (effectRow > 0)
                    {
                        _result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //發信
            }

            return _result;
        }

        /// <summary>
        /// 修改一筆場地資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // Patch: api/PlaceInfo/5
        [HttpPatch]
        //[HttpPatch("{id}")]
        public bool Patch([FromBody] SYS_PLACEINFO value)
        {
            if (!ModelState.IsValid) return false;

            if (string.IsNullOrEmpty(value.ID))
            {
                return false;
            }

            bool _result = false;
            DataTable dt = new DataTable("MyTable");
            string sql = @" UPDATE `SYS_PLACEINFO`  " +
                        "        `ClubName` = @ClubName ,"+
                        " SET    `placename` = @Placename,  " +
                        "        `placedesc` = @Placedesc,  " +
                        "        `fromdate` = @Fromdate,  " +
                        "        `todate` = @Todate,  " +
                        "        `max` = @Max,  " +
                        "        `accpet` = @Accpet,  " +
                        "        `level` = @Level,  " +
                        "        `creator` = @Creator,  " +
                        "        `creatorid` = @Creatorid,  " +
                        "        `updatedate` = @Updatedate  " +
                        " WHERE  `id` = @Id ";

            if (value.FromDate >= value.ToDate)
            {
                return false;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Id", value.ID));
                    cmd.Parameters.Add(new MySqlParameter("@ClubName", value.ClubName));
                    cmd.Parameters.Add(new MySqlParameter("@Placename", value.PlaceName));
                    cmd.Parameters.Add(new MySqlParameter("@Placedesc", value.PlaceDesc));
                    cmd.Parameters.Add(new MySqlParameter("@Fromdate", value.FromDate));
                    cmd.Parameters.Add(new MySqlParameter("@Todate", value.ToDate));
                    cmd.Parameters.Add(new MySqlParameter("@Max", value.Max));
                    cmd.Parameters.Add(new MySqlParameter("@Accpet", 'Y'));
                    cmd.Parameters.Add(new MySqlParameter("@Level", value.Level));
                    cmd.Parameters.Add(new MySqlParameter("@Creator", value.Creator));
                    cmd.Parameters.Add(new MySqlParameter("@Creatorid", value.CreatorID));
                    cmd.Parameters.Add(new MySqlParameter("@Updatedate", DateTime.Now));

                    int effectRow = cmd.ExecuteNonQuery();

                    

                    cmd.Clone();
                    cmd.Dispose();

                    if (effectRow > 0)
                    {
                        _result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //發信
            }

            return _result;
        }

        /// <summary>
        /// 刪除一筆場地資料
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{ID}")]
        public bool Delete(string ID)
        {
            bool _result = false;
            DataTable dt = new DataTable("MyTable");
            string sql = "Delete FROM `SYS_PLACEINFO` WHERE ID = @Id";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Id", ID));

                    int effectRow = cmd.ExecuteNonQuery();

                    
                    cmd.Clone();
                    cmd.Dispose();


                    if (effectRow > 0)
                    {
                        _result = true;
                    }

                }
            }
            catch (Exception ex)
            {
                //發信
            }

            return _result;
        }
    }
}
