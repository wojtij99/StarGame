using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace StarGame.Controllers
{
    public struct loginDto
    { 
        public string login { get; set; }
        public string password { get; set; }
    };

    public struct loggedDto
    { 
        public int id { get; set; }
        public string name { get; set; }
        public int mainPlanet { get; set; }
        public int AM { get; set; }
    };
    /*
    public static class mySqlCom
    {
        public static string cs { get; set; } = @"server=localhost;userid=root;password=;database=stargame;charset=utf8;";
        public static MySqlConnection con { get; set; } = new MySqlConnection(@"server=localhost;userid=root;password=;database=stargame;charset=utf8;");

        static mySqlCom() { con.Open(); }
    }
    */
    [Route("[controller]")]
    [ApiController]
    public class login : ControllerBase
    {
        

        [HttpPost]
        public ActionResult<string> loginF([FromBody] loginDto dto)
        {
            MySqlConnection conn = mySQL.mySqlOpen();

            var stm = "SELECT `ID`, `name`, `main_planet`, `AM` FROM `users` WHERE `name`='" + dto.login + "' AND `pass`='" + dto.password + "'";
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            loggedDto resultDto = new loggedDto();
            if (reader.Read())
            { 
                resultDto.id = reader.GetInt32(0);
                resultDto.name = reader.GetString(1);
                resultDto.mainPlanet = reader.GetInt32(2);
                resultDto.AM = reader.GetInt32(3);
            }
            else { reader.Close(); conn.Close(); return BadRequest("Wrong email or password"); }

            reader.Close();
            conn.Close();
            return Ok(resultDto);
        }
    }
}
