<?php
session_start();
$url = "https://localhost:8888/login";

$curl = curl_init($url);
curl_setopt($curl, CURLOPT_URL, $url);
curl_setopt($curl, CURLOPT_POST, true);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
$_SESSION['logERROR'] = "";
$headers = array(
   "Accept: application/json",
   "Content-Type: application/json",
);
curl_setopt($curl, CURLOPT_HTTPHEADER, $headers);

$login = $_POST['login'];
$pass = $_POST['pass'];

$data = <<<DATA
{
   "login": "$login",
   "password": "$pass"
}
DATA;

curl_setopt($curl, CURLOPT_POSTFIELDS, $data);

//for debug only!
curl_setopt($curl, CURLOPT_SSL_VERIFYHOST, false);
curl_setopt($curl, CURLOPT_SSL_VERIFYPEER, false);

$resp = curl_exec($curl);
$httpcode = curl_getinfo($curl, CURLINFO_HTTP_CODE);
curl_close($curl);

if($resp == null)
{
   $_SESSION['logERROR'] = "Unable connect to API";
   header('Location: index.php');
   exit();
}

if($httpcode == 400)
{
   $_SESSION['logERROR'] = $resp;
   header('Location: index.php');
   exit();
}
$Rdata = json_decode($resp);

$_SESSION['ID'] = $Rdata->id;
$_SESSION['name'] = $Rdata->name;
$_SESSION['mainPlanet'] = $Rdata->mainPlanet;
$_SESSION['AM'] = $Rdata->am;
$_SESSION['logged']=true;
header('Location: main.php');
exit();
?>