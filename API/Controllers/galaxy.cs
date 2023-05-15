using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

struct galaxyDTO
{
    public int playerID { get; set; }
    public string name { get; set; }
    public string PlayerName { get; set; }
    public DateTime lastLogin { get; set; }
    public bool isValide { get; set; }
}

namespace StarGame.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class galaxy : ControllerBase
    {
        // GET api/<galaxy>/5
        [HttpGet("{system}/{planet}")]
        public ActionResult<string> Get(int system, int planet)
        {
            if (0 <= system && system >= 100) return BadRequest("Wrong system number");
            if (0 <= planet && planet > 15) return BadRequest("Wrong planet number");

            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `player_ID`, `galaxy`.`Name`, `users`.`name`, `last_refreash` FROM `galaxy` LEFT JOIN `users` ON `galaxy`.`player_ID` = `users`.`ID` WHERE `galaxy`.`ID` = " + ((system * 15 + planet) + 1)
                    + " UNION " +
                    "SELECT `player_ID`, `galaxy`.`Name`, `users`.`name`, `last_refreash` FROM `galaxy` RIGHT JOIN `users` ON `galaxy`.`player_ID` = `users`.`ID` WHERE `galaxy`.`ID` = " + ((system * 15 + planet) + 1);
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            galaxyDTO dto = new galaxyDTO();

            if(reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    reader.Close();
                    conn.Close();
                    return Ok("NULL");
                }

                dto.playerID = reader.GetInt32(0);
                dto.name = reader.GetString(1);
                dto.PlayerName = reader.GetString(2);
                dto.lastLogin = reader.GetDateTime(3);
                dto.isValide = true;
            }
            else
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went goodn't");
            }

            reader.Close();
            conn.Close();
            return Ok(dto);
        }

        [HttpGet("{system}")]
        public ActionResult<string> Get(int system)
        {
            if (0 <= system && system >= 100) return BadRequest("Wrong system number");

            galaxyDTO[] galaxyDTOs = new galaxyDTO[15];

            for (int planet = 0; planet < 15; planet++)
            {
                MySqlConnection conn = mySQL.mySqlOpen();
                var stm = "SELECT `player_ID`, `galaxy`.`Name`, `users`.`name`, `last_refreash` FROM `galaxy` LEFT JOIN `users` ON `galaxy`.`player_ID` = `users`.`ID` WHERE `galaxy`.`ID` = " + ((system * 15 + planet) + 1) 
                    + " UNION " 
                    + "SELECT `player_ID`, `galaxy`.`Name`, `users`.`name`, `last_refreash` FROM `galaxy` RIGHT JOIN `users` ON `galaxy`.`player_ID` = `users`.`ID` WHERE `galaxy`.`ID` = " + ((system * 15 + planet) + 1);
                var cmd = new MySqlCommand(stm, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                galaxyDTO dto = new galaxyDTO();

                if (reader.Read())
                {
                    if (reader.IsDBNull(0))
                    {
                        reader.Close();
                        conn.Close();
                        dto.playerID = 0;
                        dto.name = "";
                        dto.PlayerName = "";
                        dto.lastLogin = DateTime.Now;
                        dto.isValide = false;
                        continue;
                    }

                    dto.playerID = reader.GetInt32(0);
                    dto.name = reader.GetString(1);
                    dto.PlayerName = reader.GetString(2);
                    dto.lastLogin = reader.GetDateTime(3);
                    dto.isValide = true;
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    return BadRequest("Something went goodn't");
                }

                reader.Close();
                conn.Close();
                galaxyDTOs[planet] = dto;
            }

            return Ok(galaxyDTOs);
        }

        [HttpPatch("{system}/{planet}")]
        public ActionResult<string> Patch(int system, int planet, [FromQuery]string name)
        {
            if (0 < system && system > 100) return BadRequest("Wrong system number");
            if (0 < planet && planet > 15) return BadRequest("Wrong planet number");

            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "UPDATE `galaxy` SET `Name` = '" + name + "' WHERE `ID` = " + ((system * 15 + planet) + 1);
            var cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            return Ok();
        }
    }
}
