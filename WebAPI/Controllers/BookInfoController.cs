using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookInfoController : ControllerBase
    {
        private IConfiguration _config;
        private string DBCnstr = "";
        public BookInfoController(IConfiguration config)
        {
            _config = config;
            //初始化DB連線字串
            DBCnstr = _config.GetValue<string>("ConnStr:RemoteDB");
        }

        // GET: api/BookInfo
        [HttpGet]
        public string GetBookInfoDetail()
        {
            DataTable dt = new DataTable("MyTable");
            string sql = "SELECT " +
                         "      a.*  " +
                         "  FROM" +
                         "     `SYS_BOOKINFO` a" +
                         " WHERE 1 = 1";
                         //"   AND FromDate > @now";

            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    //cmd.Parameters.Add(new MySqlParameter("@now", DateTime.Now));

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    adapter.Fill(dt);

                    cmd.Clone();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //發信
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }


            return JsonConvert.SerializeObject(dt);
        }

        // GET: api/BookInfo/123456
        [HttpGet("{id}")]
        public string GetBookInfoDetailByCreatorID(int ID)
        {
            DataTable dt = new DataTable("MyTable");
            string sql = "SELECT " +
                         "      a.*  " +
                         "  FROM" +
                         "     `SYS_BOOKINFO` a" +
                         " WHERE 1 = 1"+
                         "   AND CreatorID = @ID";

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
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }


            return JsonConvert.SerializeObject(dt);
        }

        // POST: api/BookInfo
        [HttpPost]
        public bool CreatBookInfo([FromBody] SYS_BOOKINFO value)
        {
            //驗證輸入結構是否符合
            if (!ModelState.IsValid) return false;

            bool _result = false;
            DataTable dt = new DataTable("MyTable");
            string sql = " Insert Into `SYS_BOOKINFO`  " +
                        "             (`ID`,  " +
                        "              `Name` , " +
                        "              `NickName`,  " +
                        "              `Email`,  " +
                        "              `Phone`,  " +
                        "              `Sex`,  " +
                        "              `PlaceID`,  " +
                        "              `PeopleCount`,  " +
                        "              `Creator`,  " +
                        "              `Creatorid`,  " +
                        "              `Updatedate`)  " +
                        " Values      (@ID,  " +
                        "              @Name , " +
                        "              @NickName, " +
                        "              @Email, " +
                        "              @Phone,  " +
                        "              @Sex,  " +
                        "              @PlaceID,  " +
                        "              @PeopleCount,  " +
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
                    cmd.Parameters.Add(new MySqlParameter("@Name", value.Name));
                    cmd.Parameters.Add(new MySqlParameter("@NickName", value.NickName));
                    cmd.Parameters.Add(new MySqlParameter("@Email", value.Email));
                    cmd.Parameters.Add(new MySqlParameter("@Phone", value.Phone));
                    cmd.Parameters.Add(new MySqlParameter("@Sex", value.Sex));
                    cmd.Parameters.Add(new MySqlParameter("@PlaceID", value.PlaceID));
                    cmd.Parameters.Add(new MySqlParameter("@PeopleCount", value.PeopleCount));
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
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }

            return _result;
        }

        // Patch: api/BookInfo/
        [HttpPatch]
        public bool UpdateBookInfo([FromBody] SYS_BOOKINFO value)
        {
            if (!ModelState.IsValid) return false;

            if (string.IsNullOrEmpty(value.ID))
            {
                return false;
            }

            bool _result = false;
            DataTable dt = new DataTable("MyTable");
            string sql = @" UPDATE `SYS_BOOKINFO`  " +
                        " SET    `Name` = @Name,  " +
                        "        `NickName` = @NickName,  " +
                        "        `Email` = @Email,  " +
                        "        `Phone` = @Phone,  " +
                        "        `Sex` = @Sex,  " +
                        "        `PlaceID` = @PlaceID,  " +
                        "        `PeopleCount` = @PeopleCount,  " +
                        "        `creator` = @Creator,  " +
                        "        `creatorid` = @Creatorid,  " +
                        "        `updatedate` = @Updatedate  " +
                        " WHERE  `id` = @Id ";



            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = DBCnstr;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();


                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.Add(new MySqlParameter("@Id", value.ID));
                    cmd.Parameters.Add(new MySqlParameter("@Name", value.Name));
                    cmd.Parameters.Add(new MySqlParameter("@NickName", value.NickName));
                    cmd.Parameters.Add(new MySqlParameter("@Email", value.Email));
                    cmd.Parameters.Add(new MySqlParameter("@Phone", value.Phone));
                    cmd.Parameters.Add(new MySqlParameter("@Sex", value.Sex));
                    cmd.Parameters.Add(new MySqlParameter("@PlaceID", value.PlaceID));
                    cmd.Parameters.Add(new MySqlParameter("@PeopleCount", value.PeopleCount));
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
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }

            return _result;
        }

        // DELETE: api/BookInfo/88a1e080-64da-4115-8d57-a2a865f1e732
        [HttpDelete("{ID}")]
        public bool DeleteBookInfo(string ID)
        {
            bool _result = false;
            //DataTable dt = new DataTable("MyTable");
            string sql = "Delete FROM `SYS_BOOKINFO` WHERE ID = @Id";

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
                myClass.myClass.SendMail("發生錯誤", "Exception:" + ex.Message, new List<string>());
            }

            return _result;
        }
    }
}
