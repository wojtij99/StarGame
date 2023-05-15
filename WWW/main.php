<?php
	session_start();
    if((!isset($_SESSION['logged']))||(!$_SESSION['logged']))
	{
		header('Location: index.php');
		exit();
	}
    if(!isset($_GET['page']))
    {
        header('Location: main.php?page=overview');
		exit();
    }
    require_once "settings.php";
?>
<!DOCTYPE HTML>
<html lang="pl">
	<head>
		<meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		
		<title>STAR*GAME</title>
		
		<meta name="desrciption" content="" />
		<meta name="keywords" content="" />
		<link rel="stylesheet" href="css/stylMain.css" type="text/css" />
        <link rel="stylesheet" href="css/main.css" type="text/css" />
		<link rel="stylesheet" href="css/Controlls.css" type="text/css" />
		<link href="https://fonts.googleapis.com/css?family=Press+Start+2P|VT323" rel="stylesheet"> 
		<!--<link rel="stylesheet" href="css/fontello.css" type="text/css" />-->
		<script src="js/skrypty.js"></script>   
		<!--<script src="js/timer.js"></script>-->
		<link rel="javascript" href="js/skrypty.js" type="text/javascript"/>
        <link rel="javascript" href="js/zegar.js" type="text/javascript"/>
        <link rel='javascript' href='js/ajax.js' type='text/javascript'/>
        <script src='js/ajax.js'></script>

        <script>
            window.onload = function() { refreshResource(<?php echo $_SESSION['mainPlanet'];?>); zegar();};
        </script>

	</head>
	<body>
        <article>
            <header>
                <div id="logo">
                STAR*GAME &nbsp
                </div>
                <div id="zegar"></div>
                <div>Gracz: <?php echo $_SESSION['name']; ?></div>
                <a href="logout.php"><img src="" alt="" width="20" height="20"></a>
            </header>
            <main>
                <div id="resources">
                    <div class="Material">
                        <img src="img/metal.png" alt="" width="50" height="50">
                        <div id="Metal"></div>
                    </div>
                    <div class="Material">
                        <img src="img/crystal.png" alt="" width="50" height="50">
                        <div id="Crystal"></div>
                    </div>
                    <div class="Material">
                        <img src="img/deuter.png" alt="" width="50" height="50">
                        <div id="Deuter"></div>
                    </div>
                    <div class="Material">
                        <img src="img/power.jpg" alt="" width="50" height="50">
                        <div id="Energy"></div>
                    </div>
                    <div class="Material">
                        <img src="img/Dark_matter.png" alt="" width="50" height="50">
                        <div id="AT"><?php echo $_SESSION['AM'] ?></div>
                    </div>
                    <div style="clear:both;"></div>
                </div>
                <nav>
                    <a href="?page=overview" class="linka"><div class="navItem<?php if($_GET['page'] == "overview") echo "Hover" ?>">Podgląd</div></a>
                    <a href="?page=resources" class="linka"><div class="navItem<?php if($_GET['page'] == "resources") echo "Hover" ?>">Surowce</div></a>
                    <a href="?page=station" class="linka"><div class="navItem<?php if($_GET['page'] == "station") echo "Hover" ?>">Stacja</div></a>
                    <a href="?page=traderOverview" class="linka"><div class="navItem<?php if($_GET['page'] == "traderOverview") echo "Hover" ?>">Handlarz</div></a>
                    <a href="?page=research" class="linka"><div class="navItem<?php if($_GET['page'] == "research") echo "Hover" ?>">Badania</div></a>
                    <a href="?page=shipyard" class="linka"><div class="navItem<?php if($_GET['page'] == "shipyard") echo "Hover" ?>">Stocznia</div></a>
                    <a href="?page=defenses" class="linka"><div class="navItem<?php if($_GET['page'] == "defenses") echo "Hover" ?>">Obrona</div></a>
                    <a href="?page=fleet" class="linka"><div class="navItem<?php if($_GET['page'] == "fleet") echo "Hover" ?>">Flota</div></a>
                    <a href="?page=galaxy" class="linka"><div class="navItem<?php if($_GET['page'] == "galaxy") echo "Hover" ?>">Galaktyka</div></a>
                    <a href="?page=alliance" class="linka"><div class="navItem<?php if($_GET['page'] == "alliance") echo "Hover" ?>">Sojusz</div></a>
                    <a href="?page=shop" class="linka"><div class="navItem<?php if($_GET['page'] == "shop") echo "Hover" ?>">Sklep</div></a>
                </nav>
                <div id="middle">
                    <div id="preview">
                        <img src="img/planet_background.jpg" alt="" width="630" height="300">
                    </div>
                    <?php
                        switch ($_GET['page']) 
                        {
                            case 'overview':
                                # code...
                                break;
                            case 'resources':
                                echo $PanelresourcesContent;
                                break;
                            case 'shipyard':
                                echo $PanelshipyardContent;
                                break;
                            case 'galaxy':
                                echo $PanelGalaxyContent;
                                break;
                            default:
                                ;
                                break;
                        }
                    ?>
                    <div id="queue">
                        <div class="queueItem" id="buildings">
                            <div class="queueH">Budynki:</div>
                            <img src="" alt="" width="50" height="50" id="queueIMG"> 
                            <div id="queueBuilding"></div>
                        </div>
                        <div class="queueItem" id="shipyard">
                            <div class="queueH">Stocznia:</div>
                            <img src="" alt="" width="50" height="50" id="queueShipyardIMG"> 
                            <div id="queueShipyard"></div>
                        </div>
                        <div class="queueItem" id="research">
                            <img src="" alt="" width="210" height="100">
                        </div>
                        <div style="clear:both;"></div>
                    </div>
                </div>
                <div id="right">
                    Planety...
                </div>
                <div style="clear:both;"></div>
            </main>
            <footer>
                autor:Wojciech Jędrzejewski wszelkie prawa zastrzeżone!!!
            </footer>
        </article>
		<div><img src="img/background.jpg" alt="" style="width: 100%; height: 100%; position: absolute; left: 0; top: 0; z-index: -1" /></div>
	</body>
</html>