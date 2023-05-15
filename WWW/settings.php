<?php	
	$host = "localhost";
	$db_user = "root";
	$db_password = "";
	$db_name = "stargame";

	$head = "
	<meta charset='utf-8' />
	
	<title>StarGame</title>
	
	<meta name='viewport' content='width=device-width, initial-scale=1.0'/>	
	<meta name='desrciption' content='' />
	<meta name='keywords' content='' />
	<meta name='author' content='Wojciech Jędrzejewski'/>
	<link rel='icon' href='img/LOGO.png' type='image/jpg'>
	<link rel='stylesheet' href='css/main.css' type='text/css' />
	<link rel='stylesheet' href='css/respons.css' type='text/css' />
	<link rel='stylesheet' href='css/Controlls.css' type='text/css' />
	<link rel='javascript' href='js/ajax.js' type='text/javascript'/>
    <script src='js/ajax.js'></script>
	<script type='txt/javascript' src='js/skrypty.js' defer></script>
	";

	if (isset($_SESSION['mainPlanet'])) 
	{
		$PanelresourcesContent = "
			<div id=\"panel\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/metalMine.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'metalMine')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/crystalMine.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'crystalMine')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/deuterSynthesizer.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'deuterSynthesizer')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/metalStorage.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'metalStorage')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/crystalStorage.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'crystalStorage')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/deuterTank.jpg\" title=\"\" onclick=\"showBuildingPanel(".$_SESSION['mainPlanet'].",'deuterTank')\">
			</div>
			";	
			$PanelshipyardContent = "
			<div id=\"panel\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/Light_Fighter.jpg\" title=\"\" onclick=\"showShipyardPanel(".$_SESSION['mainPlanet'].",'Light_Fighter')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/Heavy_Fighter.jpg\" title=\"\" onclick=\"showShipyardPanel(".$_SESSION['mainPlanet'].",'Heavy_Fighter')\">
				<input type=\"image\" width=\"75\" height=\"75\" src=\"img/Cruiser.jpg\" title=\"\" onclick=\"showShipyardPanel(".$_SESSION['mainPlanet'].",'Cruiser')\">
			</div>
			";	
			$PanelGalaxyContent = "
				<script> window.onload = function() { refreshResource(".$_SESSION['mainPlanet']."); zegar(); showGalaxy();}; </script>
				";
	}
?>