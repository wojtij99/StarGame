using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace StarGame.Controllers
{
    #region structs
    public struct costDTO
    {
        public int metal { get; set; }
        public int crystal { get; set; }
        public double sek { get; set; }
    }
    public struct fleetDTO
    {
        public int Small_Cargo_Ship { get; set; }
        public int Large_Cargo_Ship { get; set; }
        public int Light_Fighter { get; set; }
        public int Heavy_Fighter { get; set; }
        public int Cruiser { get; set; }
        public int Battleship { get; set; }
        public int Battlecruiser { get; set; }
        public int Bomber { get; set; }
        public int Destroyer { get; set; }
        public int Deathstar { get; set; }
        public int Recycler { get; set; }
        public int Espionage_Probe { get; set; }
        public int Solar_Satellite { get; set; }
        public int Colony_Ship { get; set; }
    }

    public struct buildFleetDTO
    {
        public int shipyard { get; set; }
        public int naniteFactory { get; set; }
        public DateTime shipyard_date { get; set; }
        public int Metal { get; set; }
        public int Crystal { get; set; }
        public int Speed { get; set; }
    }

    public struct fleetMovDTO
    {
        public int FromID { get; set; }
        public int ToID { get; set; }
        public int Small_Cargo_Ship { get; set; }
        public int Large_Cargo_Ship { get; set; }
        public int Light_Fighter { get; set; }
        public int Heavy_Fighter { get; set; }
        public int Cruiser { get; set; }
        public int Battleship { get; set; }
        public int Battlecruiser { get; set; }
        public int Bomber { get; set; }
        public int Destroyer { get; set; }
        public int Deathstar { get; set; }
        public int Recycler { get; set; }
        public int Espionage_Probe { get; set; }
        public int Solar_Satellite { get; set; }
        public int Colony_Ship { get; set; }
        public int Metal { get; set; }
        public int Crystal { get; set; }
        public int Deuter { get; set; }
    }
    #endregion

    [Route("[controller]")]
    [ApiController]
    public class fleet : ControllerBase
    {

        [HttpGet]
        public ActionResult<string> Get([FromQuery] int planet)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT * FROM `galaxy_ships` WHERE `ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            fleetDTO resultDto = new fleetDTO();
            if (reader.Read())
            {
                resultDto.Small_Cargo_Ship = reader.GetInt32(1);
                resultDto.Large_Cargo_Ship = reader.GetInt32(2);
                resultDto.Light_Fighter = reader.GetInt32(3);
                resultDto.Heavy_Fighter = reader.GetInt32(4);
                resultDto.Cruiser = reader.GetInt32(5);
                resultDto.Battleship = reader.GetInt32(6);
                resultDto.Battlecruiser = reader.GetInt32(7);
                resultDto.Destroyer = reader.GetInt32(8);
                resultDto.Deathstar = reader.GetInt32(9);
                resultDto.Recycler = reader.GetInt32(10);
                resultDto.Espionage_Probe = reader.GetInt32(11);
                resultDto.Solar_Satellite = reader.GetInt32(12);
                resultDto.Colony_Ship = reader.GetInt32(13);
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

        [HttpGet("{planet}/{ship}")]
        public ActionResult<string> Get(int planet, string ship)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `shipyard`, `naniteFactory`, `shipyard_date`, `Metal`, `Crystal`, `value` FROM `settings`, `galaxy_buildings` JOIN `galaxy` ON `galaxy_buildings`.`ID` = `galaxy`.`ID` WHERE `Key` = 'Speed' AND `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            buildFleetDTO bfDTO = new buildFleetDTO();

            if (reader.Read())
            {
                bfDTO.shipyard = reader.GetInt32(0);
                bfDTO.naniteFactory = reader.GetInt32(1);
                bfDTO.shipyard_date = reader.GetDateTime(2);
                bfDTO.Metal = reader.GetInt32(3);
                bfDTO.Crystal = reader.GetInt32(4);
                bfDTO.Speed = reader.GetInt32(5);
            }
            else
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went yesn't");
            }

            if (bfDTO.shipyard_date > DateTime.Now)
            {
                reader.Close();
                conn.Close();
                return BadRequest("Queue is full");
            }

            int targetMetal, targetCrystal;

            switch (ship)
            {
                case "Small_Cargo_Ship":
                    targetMetal = 2000;
                    targetCrystal = 2000;
                    break;
                case "Large_Cargo_Ship":
                    targetMetal = 6000;
                    targetCrystal = 6000;
                    break;
                case "Light_Fighter":
                    targetMetal = 3000;
                    targetCrystal = 1000;
                    break;
                case "Heavy_Fighter":
                    targetMetal = 6000;
                    targetCrystal = 4000;
                    break;
                case "Cruiser":
                    targetMetal = 20000;
                    targetCrystal = 7000;
                    break;
                case "Battleship":
                    targetMetal = 45000;
                    targetCrystal = 15000;
                    break;
                case "Battlecruiser":
                    targetMetal = 30000;
                    targetCrystal = 40000;
                    break;
                case "Bomber":
                    targetMetal = 50000;
                    targetCrystal = 25000;
                    break;
                case "Destroyer":
                    targetMetal = 60000;
                    targetCrystal = 50000;
                    break;
                case "Deathstar":
                    targetMetal = 5000000;
                    targetCrystal = 4000000;
                    break;
                case "Colony_Ship":
                    targetMetal = 10000;
                    targetCrystal = 20000;
                    break;
                case "Recycler":
                    targetMetal = 10000;
                    targetCrystal = 6000;
                    break;
                case "Espionage_Probe":
                    targetMetal = 0;
                    targetCrystal = 1000;
                    break;
                case "Solar_Satellite":
                    targetMetal = 0;
                    targetCrystal = 2000;
                    break;
                default:
                    reader.Close();
                    conn.Close();
                    return BadRequest("Wrong ship name");
            }
            reader.Close();

            double targetSek = (targetCrystal + targetMetal) / (2500 * (1 + bfDTO.shipyard) * bfDTO.Speed * Math.Pow(2, bfDTO.naniteFactory)) * 60 * 60;

            costDTO cDTO = new costDTO();
            cDTO.metal = targetMetal;
            cDTO.crystal = targetCrystal;
            cDTO.sek = targetSek;

            conn.Close();
            return Ok(cDTO);
        }

        [HttpPatch("{planet}/{ship}")]
        public ActionResult<string> Patch(int planet, string ship, [FromQuery]int count)
        {
            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `shipyard`, `naniteFactory`, `shipyard_date`, `Metal`, `Crystal`, `value` FROM `settings`, `galaxy_buildings` JOIN `galaxy` ON `galaxy_buildings`.`ID` = `galaxy`.`ID` WHERE `Key` = 'Speed' AND `galaxy`.`ID` = " + planet;
            var cmd = new MySqlCommand(stm, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            buildFleetDTO bfDTO = new buildFleetDTO();

            if(reader.Read())
            {
                bfDTO.shipyard = reader.GetInt32(0);
                bfDTO.naniteFactory = reader.GetInt32(1);
                bfDTO.shipyard_date = reader.GetDateTime(2);
                bfDTO.Metal = reader.GetInt32(3);
                bfDTO.Crystal = reader.GetInt32(4);
                bfDTO.Speed = reader.GetInt32(5);
            }
            else
            {
                reader.Close();
                conn.Close();
                return BadRequest("Something went yesn't");
            }

            if (bfDTO.shipyard_date > DateTime.Now)
            {
                reader.Close();
                conn.Close();
                return BadRequest("Queue is full");
            }

            int targetMetal, targetCrystal;

            switch (ship)
            {
                case "Small_Cargo_Ship":
                    targetMetal = 2000;
                    targetCrystal = 2000;
                    break;
                case "Large_Cargo_Ship":
                    targetMetal = 6000;
                    targetCrystal = 6000;
                    break;
                case "Light_Fighter":
                    targetMetal = 3000;
                    targetCrystal = 1000;
                    break;
                case "Heavy_Fighter":
                    targetMetal = 6000;
                    targetCrystal = 4000;
                    break;
                case "Cruiser":
                    targetMetal = 20000;
                    targetCrystal = 7000;
                    break;
                case "Battleship":
                    targetMetal = 45000;
                    targetCrystal = 15000;
                    break;
                case "Battlecruiser":
                    targetMetal = 30000;
                    targetCrystal = 40000;
                    break;
                case "Bomber":
                    targetMetal = 50000;
                    targetCrystal = 25000;
                    break;
                case "Destroyer":
                    targetMetal = 60000;
                    targetCrystal = 50000;
                    break;
                case "Deathstar":
                    targetMetal = 5000000;
                    targetCrystal = 4000000;
                    break;
                case "Colony_Ship":
                    targetMetal = 10000;
                    targetCrystal = 20000;
                    break;
                case "Recycler":
                    targetMetal = 10000;
                    targetCrystal = 6000;
                    break;
                case "Espionage_Probe":
                    targetMetal = 0;
                    targetCrystal = 1000;
                    break;
                case "Solar_Satellite":
                    targetMetal = 0;
                    targetCrystal = 2000;
                    break;
                default:
                    reader.Close();
                    conn.Close();
                    return BadRequest("Wrong ship name");
            }
            reader.Close();

            if (bfDTO.Metal - targetMetal < 0 || bfDTO.Crystal - targetCrystal < 0) { conn.Close(); return BadRequest("Not enough resources"); }

            double targetHour = (targetCrystal + targetMetal) / (2500 * (1 + bfDTO.shipyard) * bfDTO.Speed * Math.Pow(2, bfDTO.naniteFactory));
            DateTime targetDate = DateTime.Now.AddHours(targetHour);

            stm = "UPDATE `galaxy` SET `Metal` = `Metal` - " + targetMetal + ", `Crystal` = `Crystal` - " + targetCrystal + ", `shipyard_queue` = '" + ship + "', `shipyard_date` = '" + targetDate.ToString("yyyy-MM-dd HH-mm-ss") + "' WHERE `ID` = " + planet;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            reader.Close();
            conn.Close();
            return Ok();
        }

        [HttpPut]
        public ActionResult<string> Put([FromBody] fleetMovDTO movDTO)
        {
            //if((movDTO.Metal + movDTO.Crystal + movDTO.Deuter) >  )

            MySqlConnection conn = mySQL.mySqlOpen();
            var stm = "SELECT `value` FROM `settings` WHERE `Key` = 'Speed'";
            var cmd = new MySqlCommand(stm, conn);
            int universSpeed = Convert.ToInt32(cmd.ExecuteScalar());

            int fleetEfficiency = 100;
            //Actual Speed = (Base Speed) * (1 + ((Drive level) * (Drive bonus factor)))
            int baseSpeed = 0;

            int speed = 10000;

            int distance = 0;
            if(movDTO.ToID == movDTO.FromID) distance = 5;
            else 
            {
                int preDistance = Math.Abs(movDTO.ToID - movDTO.FromID);
                distance = (2700 + 95 * Convert.ToInt32(preDistance / 15)) + (1000 + 5 * (preDistance % 15));
            }

            //ROUND((35'000 / % * (Distance * 1'000 / (fleet speed)) ^(1 / 2) + 10) / (Universe speed))
            DateTime dtDeparture = DateTime.Now;
            DateTime dtArrival = DateTime.Now.AddSeconds(Math.Round(35000 / fleetEfficiency * Math.Pow((distance * 1000 / speed), 0.5) + 10) / universSpeed);

            stm = "INSERT INTO `fleet` VALUES(NULL, " + movDTO.FromID + ", " + movDTO.ToID + ", 'transport', '" + dtDeparture.ToString("yyyy-MM-dd HH-mm-ss") + "', '" 
                + dtArrival.ToString("yyyy-MM-dd HH-mm-ss") + "', "+ movDTO.Small_Cargo_Ship + ", " + movDTO.Large_Cargo_Ship + ", " + movDTO.Light_Fighter + ", "
                + movDTO.Heavy_Fighter + ", " + movDTO.Cruiser + ", " + movDTO.Battleship + ", " + movDTO.Battlecruiser + ", " + movDTO.Bomber + ", "
                + movDTO.Destroyer + ", " + movDTO.Deathstar + ", " + movDTO.Recycler + ", " + movDTO.Espionage_Probe + ", " + movDTO.Solar_Satellite + ", " 
                + movDTO.Colony_Ship + ", " + movDTO.Metal + ", " + movDTO.Crystal + ", " + movDTO.Destroyer + " )";
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            stm = "UPDATE `galaxy` JOIN `galaxy_ships` ON `galaxy_ships`.`ID` = `galaxy`.`ID` SET `Metal`= `Metal` - " + movDTO.Metal +
                ", `Crystal`= `Crystal` - " + movDTO.Crystal + ",`Deuter`= `Deuter` - " + movDTO.Deuter + ",`Small_Cargo_Ship`= `Small_Cargo_Ship` - " + movDTO.Small_Cargo_Ship +
                ", `Large_Cargo_Ship`= `Large_Cargo_Ship` - " + movDTO.Large_Cargo_Ship + ", `Light_Fighter`= `Light_Fighter` - " + movDTO.Light_Fighter +
                ", `Heavy_Fighter` = `Heavy_Fighter` - " + movDTO.Heavy_Fighter + ", `Cruiser` = `Cruiser` - " + movDTO.Cruiser + ",`Battleship` = `Battleship` - " + movDTO.Battleship +
                ", `Battlecruiser` = `Battlecruiser` - " + movDTO.Battlecruiser + ", `Bomber` = `Bomber` - " + movDTO.Bomber + ", `Destroyer` = `Destroyer` - " + movDTO.Destroyer +
                ", `Deathstar` = `Deathstar` - " + movDTO.Deathstar + ", `Recycler` = `Recycler` - " + movDTO.Recycler + 
                ", `Espionage_Probe` = `Espionage_Probe` - " + movDTO.Espionage_Probe + ", `Solar_Satellite` = `Solar_Satellite` - " + movDTO.Solar_Satellite +
                ", `Colony_Ship` = `Colony_Ship` - " + movDTO.Colony_Ship + " WHERE `galaxy`.`ID` = " + movDTO.FromID;
            cmd = new MySqlCommand(stm, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            return Ok();
        }
    }
}
    