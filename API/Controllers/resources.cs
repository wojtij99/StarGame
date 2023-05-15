using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarGame.Controllers
{
    public struct resourcesDto
    {
        public double Metal { get; set; }
        public double Crystal { get; set; }
        public double Deuter { get; set; }
        public double construction_sek { get; set; }
        public string construction { get; set; }
        public double shipyard_sek { get; set; }
        public string shipyard { get; set; }
    }

    public struct refreashDto
    {
        public double Metal { get; set; }
        public double Crystal { get; set; }
        public double Deuter { get; set; }
        public int MetalMine { get; set; }
        public int CrystalMine { get; set; }
        public int DeuterSynthesizer { get; set; }
        public int MetalStorage { get; set; }
        public int CrystalStorage { get; set; }
        public int DeuterTank { get; set; }
        public int SolarPlant { get; set; }
        public DateTime time { get; set; }
        public DateTime construction_date { get; set; }
        public string construction { get; set; }
        public DateTime shipyard_date { get; set; }
        public string shipyard_queue { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class resources : ControllerBase
    {
        string cs = @"server=localhost;userid=root;password=;database=stargame;charset=utf8;";

        // GET: api/<resources>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [HttpGet]
        public ActionResult<string> Get([FromQuery]int planet)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `Metal`, `Crystal`, `Deuter` FROM `galaxy` WHERE `ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            resourcesDto resultDto = new resourcesDto();
            if (reader.Read())
            {
                resultDto.Metal = reader.GetInt32(0);
                resultDto.Crystal = reader.GetInt32(1);
                resultDto.Deuter = reader.GetInt32(2);
            }
            else { reader.Close(); conn.Close(); return BadRequest("Something went yesn't"); }

            reader.Close();
            conn.Close();
            return Ok(resultDto);
        }

        // GET api/<resources>/5
        [HttpGet("{resource}")]
        public ActionResult<int> Get([FromQuery] int planet, string resource)
        {
            if (!(resource == "Metal" || resource == "Crystal" || resource == "Deuter")) return BadRequest("It's not a resource!");

            MySqlConnection conn = mySQL.mySqlOpen();

            var stm = "SELECT `" + resource + "` FROM `galaxy` WHERE `ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            int result = int.Parse(cmd.ExecuteScalar().ToString());

            conn.Close();
            return Ok(result);
        }

        [HttpPatch] //REFREASH RESOURCE
        public ActionResult<string> Patch([FromQuery] int planet, bool construct = false)
        {
            //pobierania

            MySqlConnection conn = mySQL.mySqlOpen();

            var stm = "SELECT `Metal`, `Crystal`, `Deuter`, `metalMine`, `crystalMine`, `deuterSynthesizer`, `metalStorage`, `crystalStorage`, `deuterTank`, `solarPlant`, `last_refreash`, `construction_date`, `construction`, `shipyard_date`, `shipyard_queue` FROM `galaxy` JOIN `galaxy_buildings` ON `galaxy`.`ID` = `galaxy_buildings`.`ID` WHERE `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            /*for (int i = 1; i < 1501; i++)
            {
                stm = "INSERT INTO `galaxy_ships` VALUES ("+i+",0,0,0,0,0,0,0,0,0,0,0,0,0,0)";
                cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
            }

            stm = "SELECT `Metal`, `Crystal`, `Deuter`, `metalMine`, `crystalMine`, `deuterSynthesizer`, `metalStorage`, `crystalStorage`, `deuterTank`, `solarPlant`, `last_refreash`, `construction_date`, `construction` FROM `galaxy` JOIN `galaxy_buildings` ON `galaxy`.`ID` = `galaxy_buildings`.`ID` WHERE `galaxy`.`ID` = " + planet;
            cmd = new MySqlCommand(stm, conn);*/

            MySqlDataReader reader = cmd.ExecuteReader();
            refreashDto refdto = new refreashDto(); 
            if (reader.Read())
            {
                refdto.Metal = reader.GetDouble(0);
                refdto.Crystal = reader.GetDouble(1);
                refdto.Deuter = reader.GetDouble(2);
                refdto.MetalMine = reader.GetInt32(3);
                refdto.CrystalMine = reader.GetInt32(4);
                refdto.DeuterSynthesizer = reader.GetInt32(5);
                refdto.MetalStorage = reader.GetInt32(6);
                refdto.CrystalStorage = reader.GetInt32(7);
                refdto.DeuterTank = reader.GetInt32(8);
                refdto.SolarPlant = reader.GetInt32(9);
                refdto.time = reader.GetDateTime(10);
                refdto.construction_date = reader.GetDateTime(11);
                refdto.construction = reader.GetString(12);
                refdto.shipyard_date = reader.GetDateTime(13);
                refdto.shipyard_queue = reader.GetString(14);
            }
            else 
            { 
                reader.Close(); 
                conn.Close(); 
                return BadRequest("Something went yesn't"); 
            }
            reader.Close();

            //Kopalnie
             
            resourcesDto refRes = new resourcesDto();

            refRes.construction = refdto.construction;
            refRes.construction_sek = (refdto.construction_date - DateTime.Now).TotalSeconds;
            refRes.shipyard = refdto.shipyard_queue;
            refRes.shipyard_sek = (refdto.shipyard_date - DateTime.Now).TotalSeconds;

            double time = 0;
            DateTime dateTime = DateTime.Now;

            if (DateTime.Now > refdto.construction_date && !construct && refdto.construction != "" && refdto.construction != null)
            {
                time = double.Parse(refdto.time.Subtract(refdto.construction_date).TotalHours.ToString()) * -1;
                dateTime = refdto.construction_date;

                stm = "UPDATE `galaxy` JOIN `galaxy_buildings` ON `galaxy_buildings`.`ID` = `galaxy`.`ID` SET `" + refdto.construction + "` = `" + refdto.construction + "` + 1, `construction` = NULL  WHERE `galaxy`.`ID` = " + planet;
                cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
            }
            else time = double.Parse(refdto.time.Subtract(DateTime.Now).TotalHours.ToString()) * -1;

            if (DateTime.Now > refdto.shipyard_date && refdto.shipyard_queue != "" && refdto.shipyard_queue != null) 
            {
                stm = "UPDATE `galaxy` JOIN `galaxy_ships` ON `galaxy`.`ID` = `galaxy_ships`.`ID` SET `" + refdto.shipyard_queue + "` = `" + refdto.shipyard_queue + "` + 1, `shipyard_queue` = NULL WHERE `galaxy`.`ID` = " + planet;
                cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
            }

            // INT(2.5 * e ^ (20 * (level) / 33)) * 5'000

            int metalCap = Convert.ToInt32(2.5 * Math.Pow(Math.E, (20 * refdto.MetalStorage / 33)) * 5000);
            int crystalCap = Convert.ToInt32(2.5 * Math.Pow(Math.E, (20 * refdto.CrystalStorage / 33)) * 5000); 
            int deuterCap = Convert.ToInt32(2.5 * Math.Pow(Math.E, (20 * refdto.DeuterTank / 33)) * 5000);

            refRes.Metal = refdto.Metal;
            if (refdto.Metal < metalCap)
            {
                refRes.Metal += double.Parse((30 * refdto.MetalMine * Math.Pow(1.1, refdto.MetalMine)).ToString()) * time;
                if(refRes.Metal > metalCap) refRes.Metal = metalCap;
            }

            refRes.Crystal = refdto.Crystal;
            if (refdto.Crystal < crystalCap) 
            {
                refRes.Crystal += double.Parse((20 * refdto.CrystalMine * Math.Pow(1.1, refdto.CrystalMine)).ToString()) * time;
                if (refRes.Crystal > crystalCap) refRes.Crystal = crystalCap;
            }

            refRes.Deuter = refdto.Deuter;
            if (refdto.Deuter < deuterCap)
            { 
                refRes.Deuter += double.Parse((10 * refdto.DeuterSynthesizer * Math.Pow(1.1, refdto.DeuterSynthesizer)).ToString()) * time;
                if (refRes.Deuter > deuterCap) refRes.Deuter = deuterCap;
            }

            //Akcje flot

            //Transport
            stm = "SELECT * FROM `fleet` WHERE `Arrival` <= now() AND (`FromID` = " + planet + " OR `ToID` = " + planet + ");";
            cmd = new MySqlCommand(stm, conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fleetMovDTO movDTO = new fleetMovDTO();
                int id = reader.GetInt32(0);
                movDTO.FromID = reader.GetInt32(1);
                movDTO.ToID = reader.GetInt32(2);
                string mission = reader.GetString(3);
                movDTO.Small_Cargo_Ship = reader.GetInt32(6);
                movDTO.Large_Cargo_Ship = reader.GetInt32(7);
                movDTO.Light_Fighter = reader.GetInt32(8);
                movDTO.Heavy_Fighter = reader.GetInt32(9);
                movDTO.Cruiser = reader.GetInt32(10);
                movDTO.Battleship = reader.GetInt32(11);
                movDTO.Battlecruiser = reader.GetInt32(12);
                movDTO.Bomber = reader.GetInt32(13);
                movDTO.Destroyer = reader.GetInt32(14);
                movDTO.Deathstar = reader.GetInt32(15);
                movDTO.Recycler = reader.GetInt32(16);
                movDTO.Espionage_Probe = reader.GetInt32(17);
                movDTO.Solar_Satellite = reader.GetInt32(18);
                movDTO.Colony_Ship = reader.GetInt32(19);
                movDTO.Metal = reader.GetInt32(20);
                movDTO.Crystal = reader.GetInt32(21);
                movDTO.Deuter = reader.GetInt32(22);

                MySqlConnection conn2 = mySQL.mySqlOpen();

                var stm2 = "UPDATE `galaxy` SET `Metal` = `Metal` + " + movDTO.Metal + ", `Crystal` = `Crystal` + " + movDTO.Crystal + ", `Deuter` = `Deuter` + " + movDTO.Deuter
                    + " WHERE `ID` = " + movDTO.ToID;
                var cmd2 = new MySqlCommand(stm2, conn2);
                cmd2.ExecuteNonQuery();

                DateTime dtDeparture = reader.GetDateTime(5);
                DateTime dtArrival = dtDeparture.Add(reader.GetDateTime(4) - dtDeparture);

                if (mission == "transport")
                    stm2 = "INSERT INTO `fleet` VALUES(NULL, " + movDTO.ToID + ", " + movDTO.FromID + ", 'return', '" + dtDeparture.ToString("yyyy-MM-dd HH-mm-ss") + "', '"
                        + dtArrival.ToString("yyyy-MM-dd HH-mm-ss") + "', " + movDTO.Small_Cargo_Ship + ", " + movDTO.Large_Cargo_Ship + ", " + movDTO.Light_Fighter + ", "
                        + movDTO.Heavy_Fighter + ", " + movDTO.Cruiser + ", " + movDTO.Battleship + ", " + movDTO.Battlecruiser + ", " + movDTO.Bomber + ", "
                        + movDTO.Destroyer + ", " + movDTO.Deathstar + ", " + movDTO.Recycler + ", " + movDTO.Espionage_Probe + ", " + movDTO.Solar_Satellite + ", "
                        + movDTO.Colony_Ship + ", 0, 0, 0);";
                else if (mission == "return")
                    stm2 = "UPDATE `galaxy` JOIN `galaxy_ships` ON `galaxy_ships`.`ID` = `galaxy`.`ID` SET `Small_Cargo_Ship`= `Small_Cargo_Ship` + " + movDTO.Small_Cargo_Ship +
                        ", `Large_Cargo_Ship`= `Large_Cargo_Ship` + " + movDTO.Large_Cargo_Ship + ", `Light_Fighter`= `Light_Fighter` + " + movDTO.Light_Fighter +
                        ", `Heavy_Fighter` = `Heavy_Fighter` + " + movDTO.Heavy_Fighter + ", `Cruiser` = `Cruiser` + " + movDTO.Cruiser + ",`Battleship` = `Battleship` + " + movDTO.Battleship +
                        ", `Battlecruiser` = `Battlecruiser` + " + movDTO.Battlecruiser + ", `Bomber` = `Bomber` + " + movDTO.Bomber + ", `Destroyer` = `Destroyer` + " + movDTO.Destroyer +
                        ", `Deathstar` = `Deathstar` + " + movDTO.Deathstar + ", `Recycler` = `Recycler` + " + movDTO.Recycler +
                        ", `Espionage_Probe` = `Espionage_Probe` + " + movDTO.Espionage_Probe + ", `Solar_Satellite` = `Solar_Satellite` + " + movDTO.Solar_Satellite +
                        ", `Colony_Ship` = `Colony_Ship` + " + movDTO.Colony_Ship + " WHERE `galaxy`.`ID` = " + movDTO.ToID;
                cmd2 = new MySqlCommand(stm2, conn2);
                cmd2.ExecuteNonQuery();

                stm2 = "DELETE FROM `fleet` WHERE `ID` = " + id;
                cmd2 = new MySqlCommand(stm2, conn2);
                cmd2.ExecuteNonQuery();

                conn2.Close();
            }
            reader.Close();

            stm = "UPDATE `galaxy` JOIN `galaxy_buildings` ON `galaxy_buildings`.`ID` = `galaxy`.`ID` SET `Metal` = '" + refRes.Metal + "', `Crystal` = '" + refRes.Crystal + "', `Deuter` = '" + refRes.Deuter + "', `last_refreash` = '" + dateTime.ToString("yyyy-MM-dd HH-mm-ss") + "' WHERE `galaxy`.`ID` = " + planet;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (dateTime == refdto.construction_date) return Patch(planet, true);
            return Ok(refRes);
        }

        /*
        // PATCH 
        [HttpPatch]
        public ActionResult<string> Patch([FromQuery] int planet, [FromBody] resourcesDto resourcesDto)
        {
            var stm = "UPDATE `galaxy` SET `Metal` = '" + resourcesDto.Metal + "', `Crystal` = '" + resourcesDto.Crystal + "', `Deuter` = '" + resourcesDto.Deuter + "' WHERE `ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            return Ok();
        }

        // PATCH
        [HttpPatch("{resource}")]
        public ActionResult<int> Patch([FromQuery] int planet, [FromQuery] int num, string resource)
        {
            if (!(resource == "Metal" || resource == "Crystal" || resource == "Deuter")) return BadRequest("It's not a resource!");

            var stm = "UPDATE `galaxy` SET " + resource + " = '"+num+"' WHERE `ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            return Ok();
        }
        */
        /*// POST api/<resources>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        
        // PUT api/<resources>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<resources>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
