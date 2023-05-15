using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace StarGame.Controllers
{
    public struct registerDto
    {
        public string nick { get; set; }
        public string email { get; set; }
        public string pass1 { get; set; }
        public string pass2 { get; set; }
        public bool regul { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class register : ControllerBase
    {
        string cs = @"server=localhost;userid=root;password=;database=stargame;charset=utf8;";

        [HttpGet("reg")]
        public ActionResult<string> TEST()
        {
           return BadRequest("test");
        }

        [HttpPost]
        public ActionResult REG([FromBody] registerDto dto)
        {
            Console.WriteLine(dto.nick);
            Console.WriteLine(dto.email);
            Console.WriteLine(dto.pass1);
            Console.WriteLine(dto.pass2);
            Console.WriteLine(dto.regul);

            if (!dto.regul) return BadRequest("Regulamin is not accepted");
            if (dto.nick.Length < 3) return BadRequest("nick is too short");
            if (dto.pass1.Length < 8) return BadRequest("password is too short");
            if (dto.pass1 != dto.pass2) return BadRequest("pass1 not equal pass2");

            MySqlConnection conn = mySQL.mySqlOpen();

            var stm = "SELECT `ID` FROM `users` WHERE `email`='" + dto.email + "';";
            var cmd = new MySqlCommand(stm, conn);

            if (cmd.ExecuteScalar() != null) { conn.Close(); return BadRequest("Account connected to this email is existing"); }
            
            stm = "SELECT `ID` FROM `users` WHERE `name`= '" + dto.nick + "';";
            cmd = new MySqlCommand(stm, conn);

            if (cmd.ExecuteScalar() != null)
            {
                conn.Close(); return BadRequest("This nick is occupied");
            }

            try { var addr = new System.Net.Mail.MailAddress(dto.email);}
            catch{ conn.Close(); return BadRequest("email is not valid");}

            stm = "INSERT INTO `users` VALUES (NULL,'" + dto.nick + "','" + dto.pass1 + "','" + dto.email + "',10000, NULL);";
            cmd = new MySqlCommand (stm, conn);
            cmd.ExecuteNonQuery();

            stm = "SELECT LAST_INSERT_ID() AS XD;";
            cmd = new MySqlCommand(stm, conn);
            int ID_player = int.Parse(cmd.ExecuteScalar().ToString());

            bool isLos = false;

            Random rnd = new Random();
            int planet = rnd.Next(1, 1500);

            while (isLos)
            {
                stm = "SELECT `ID` FROM `galaxy` WHERE `ID`= " + planet + ";";
                cmd = new MySqlCommand(stm, conn);

                if (cmd.ExecuteScalar() == null) isLos = true;
            }

            stm = "UPDATE galaxy SET Name = '" + dto.nick + "', Metal = 1000, Crystal = 1000, player_ID=" + ID_player + ", last_refreash=now() WHERE id=" + planet;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            stm = "UPDATE users SET main_planet=" + planet + " WHERE ID=" + ID_player;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            return Ok(dto);
        }
    }
}
