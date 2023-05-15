using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarGame
{
    public struct buildingsDTO
    { 
        public int metalMine { get; set; }
        public int crystalMine { get; set; }
        public int deuterSynthesizer { get; set; }
        public int metalStorage { get; set; }
        public int crystalStorage { get; set; }
        public int deuterTank { get; set; }
        public int solarPlant { get; set; }
        public int robotsFactory { get; set; } 
        public int shipyard { get; set; }
        public int researchLab { get; set; }
        public int naniteFactory { get; set; }
    }

    public struct constructionDTO
    {
        public int Metal { get; set; }
        public int Crystal { get; set; }
        public int robotsFactory { get; set; }
        public int naniteFactory { get; set; } 
        public int lvl { get; set; }
        public DateTime construction_date { get; set; }
        public int Speed { get; set; }
    }

    public struct costDTO
    {
        public int metal { get; set; }
        public int crystal { get; set; }
        public double sek { get; set; }
        public int lvl { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class buildings : ControllerBase
    {
        // GET: api/<buildings>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET api/<buildings>/5
        [HttpGet]
        public ActionResult<string> Get([FromQuery] int planet)
        {
            
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `metalMine`, `crystalMine`, `deuterSynthesizer`, `metalStorage`, `crystalStorage`, `deuterTank`, `solarPlant`, `robotsFactory`, `shipyard`, `naniteFactory`, `researchLab` FROM `galaxy` JOIN `galaxy_buildings` ON `galaxy`.`ID` = `galaxy_buildings`.`ID` WHERE `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            buildingsDTO resultDto = new buildingsDTO();
            if (reader.Read())
            {
                resultDto.metalMine = reader.GetInt32(0);
                resultDto.crystalMine = reader.GetInt32(1);
                resultDto.deuterSynthesizer = reader.GetInt32(2);
                resultDto.metalStorage = reader.GetInt32(3);
                resultDto.crystalStorage = reader.GetInt32(4);
                resultDto.deuterTank = reader.GetInt32(5);
                resultDto.solarPlant = reader.GetInt32(6);
                resultDto.robotsFactory = reader.GetInt32(7);
                resultDto.shipyard = reader.GetInt32(8);
                resultDto.researchLab = reader.GetInt32(9);
                resultDto.naniteFactory = reader.GetInt32(10);
            }
            else 
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went yesn't"); 
            }

            reader.Close();
            conn.Close();
            return Ok(resultDto);
        }

        [HttpGet("{building}")]
        public ActionResult<string> Get([FromQuery] int planet, string building)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `" + building + "`, `Metal`, `Crystal`, `robotsFactory`, `naniteFactory`, `construction_date`, `value` FROM `settings`, `galaxy` JOIN `galaxy_buildings` ON `galaxy`.`ID` = `galaxy_buildings`.`ID` WHERE `Key` = 'Speed' AND `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            constructionDTO conDTO = new constructionDTO();
            if (reader.Read())
            {
                conDTO.lvl = reader.GetInt32(0);
                conDTO.Metal = reader.GetInt32(1);
                conDTO.Crystal = reader.GetInt32(2);
                conDTO.robotsFactory = reader.GetInt32(3);
                conDTO.naniteFactory = reader.GetInt32(4);
                conDTO.construction_date = reader.GetDateTime(5);
                conDTO.Speed = reader.GetInt32(6);
            }
            else
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went goodn't");
            }
            reader.Close();

            /*if (conDTO.construction_date > DateTime.Now)
            {
                conn.Close();
                return BadRequest("Queue is full");
            }*/

            int targetMetal, targetCrystal;

            conDTO.lvl += 1;

            switch (building)
            {
                case "metalMine":
                    targetMetal = Convert.ToInt32(60 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(15 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "crystalMine":
                    targetMetal = Convert.ToInt32(48 * Math.Pow(1.6, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(24 * Math.Pow(1.6, (conDTO.lvl - 1)));
                    break;
                case "deuterSynthesizer":
                    targetMetal = Convert.ToInt32(255 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(75 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "solarPlant":
                    targetMetal = Convert.ToInt32(75 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(30 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "metalStorage":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(0 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "crystalStorage":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(500 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "deuterTank":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "robotsFactory":
                    targetMetal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(120 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "shipyard":
                    targetMetal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(200 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "researchLab":
                    targetMetal = Convert.ToInt32(200 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "naniteFactory":
                    targetMetal = Convert.ToInt32(1000000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(500000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                default:
                    conn.Close();
                    return BadRequest("Wrong building name");
            }

            double targetSek = (targetCrystal + targetMetal) / (2500 * (1 + conDTO.robotsFactory) * conDTO.Speed * Math.Pow(2, conDTO.naniteFactory)) * 60 * 60;

            costDTO cost = new costDTO();
            cost.metal = targetMetal;
            cost.crystal = targetCrystal;
            cost.sek = targetSek;
            cost.lvl = conDTO.lvl;

            conn.Close();
            return Ok(cost);
        }

        [HttpPatch("{building}")]
        public ActionResult<string> Patch([FromQuery] int planet, string building)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `" + building + "`, `Metal`, `Crystal`, `robotsFactory`, `naniteFactory`, `construction_date`, `value` FROM `settings`, `galaxy` JOIN `galaxy_buildings` ON `galaxy`.`ID` = `galaxy_buildings`.`ID` WHERE `Key` = 'Speed' AND `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            constructionDTO conDTO = new constructionDTO();
            if (reader.Read())
            {
                conDTO.lvl = reader.GetInt32(0);
                conDTO.Metal = reader.GetInt32(1);
                conDTO.Crystal = reader.GetInt32(2);
                conDTO.robotsFactory = reader.GetInt32(3);
                conDTO.naniteFactory = reader.GetInt32(4);
                conDTO.construction_date = reader.GetDateTime(5);
                conDTO.Speed = reader.GetInt32(6);
            }
            else
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went goodn't");
            }
            reader.Close();

            if (conDTO.construction_date > DateTime.Now)
            {
                conn.Close();
                return BadRequest("Queue is full");
            }

            int targetMetal, targetCrystal;

            switch(building)
            {
                case "metalMine":
                    targetMetal = Convert.ToInt32(60 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(15 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "crystalMine":
                    targetMetal = Convert.ToInt32(48 * Math.Pow(1.6, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(24 * Math.Pow(1.6, (conDTO.lvl - 1)));
                    break;
                case "deuterSynthesizer":
                    targetMetal = Convert.ToInt32(255 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(75 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "solarPlant":
                    targetMetal = Convert.ToInt32(75 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(30 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "metalStorage":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(0 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "crystalStorage":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(500 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "deuterTank":
                    targetMetal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(1000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "robotsFactory":
                    targetMetal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(120 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "shipyard":
                    targetMetal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(200 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "researchLab":
                    targetMetal = Convert.ToInt32(200 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(400 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                case "naniteFactory":
                    targetMetal = Convert.ToInt32(1000000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    targetCrystal = Convert.ToInt32(500000 * Math.Pow(1.5, (conDTO.lvl - 1)));
                    break;
                default:
                    conn.Close();
                    return BadRequest("Wrong building name");
            }

            if (conDTO.Metal - targetMetal < 0 || conDTO.Crystal - targetCrystal < 0) { conn.Close(); return BadRequest("Not enough resources"); }

            double targetHour = (targetCrystal + targetMetal)/(2500 * (1 + conDTO.robotsFactory) * conDTO.Speed * Math.Pow(2, conDTO.naniteFactory));
            DateTime targetDate = DateTime.Now.AddHours(targetHour);

            stm = "UPDATE `galaxy` SET `Metal` = `Metal` - " + targetMetal + ", `Crystal` = `Crystal` - " + targetCrystal + ", `construction` = '" + building + "', `construction_date` = '" + targetDate.ToString("yyyy-MM-dd HH-mm-ss") + "' WHERE `ID` = " + planet;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();
            
            conn.Close();
            return Ok();
        }

        /*// POST api/<buildings>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<buildings>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<buildings>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
