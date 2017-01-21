<?
// CR.com id
$clientId = '800ci-9f9b1a97-4dc9-4f38-a4d4-644a9bfc1eb3';
// CR.com Shared Secret
$shared_secret = "800ss-860f1f6b-38b6-41b2-9c78-ba4973d545af";
// Step 1: Get usertoken with authenticateuser
// username and password
$user = 'crtest1@criminalrecords.com'; 
$pass = '100100th'; 

$authProxyUrl = "http://integ-jweb11.tuk2.intelius.com:8080/";
$authProxyServiceUri = "authproxy-1.1.6/";
$timestamp = date("Y-m-d H:i:s");
$path = "4.0/account/authenticateuser";
$endUserIp = $_SERVER['REMOTE_ADDR']; // Or maybe use HTTP_X_FORWARDED_FOR

// Generate hashed signed request
// Order of fields *is* important
$encodedQueryString = $path."?client_id=".$clientId."&timestamp=".urlencode($timestamp);
$encodedFormString = "email_address=".urlencode($user)."&password=".urlencode($pass)."&remote_ip=".$endUserIp;
$signedRequest = hash("sha256", $encodedQueryString.$encodedFormString.$shared_secret);

$encodedQueryString .= "&signed_request=".$signedRequest;
$authenticationPostRequest = array("email_address" => $user, "password" => $pass, "remote_ip" => $endUserIp);

// Make POST request
// query params - clientId, timestamp, signed_request
// Everything else if form params

// Authenticate User ID/Password.
echo "1. POST to AuthProxy ($authProxyUrl.$authProxyServiceUri.$encodedQueryString) with Formparams ($authenticationPostRequest) \n";
$getAuthResponse = lib_Util_Service::request($authProxyServiceUri.$encodedQueryString, $authProxyUrl, $authenticationPostRequest);

if (UTIL_IsEmpty($getAuthResponse, 'userAuthentication', 'userToken') || $getAuthResponse['responseCode'] != 1000)
{
   print("Error authenicating user.");
   ?>
      <pre><?print_r($getAuthResponse);?></pre>
   <?
   exit;
}

$t = $getAuthResponse['userAuthentication']['userToken'];
$u = $getAuthResponse['userAuthentication']['userId'];


// Connect to PaymentProxy
// Step 2: Get nonce (15 min)
$paymentProxyUrl = "http://integ-jweb11.tuk2.intelius.com:8080/"; // e.g.: http://<server>:8080/
$initalRequestPostArr = array('clientid' => $clientId, 'enduserip' => $endUserIp);
$paymentProxyUri = "paymentproxy-0.0.2";
echo "2. POST to PaymentProxy ($paymentProxyUrl.$paymentProxyUri/1.0/getnonce) with Formparams($initalRequestPostArr) \n";
$getNonceResponse = lib_Util_Service::request("paymentproxy-0.0.2/1.0/getnonce", $paymentProxyUrl, $initalRequestPostArr);
$nonce = $getNonceResponse['nonce']['nonce'];
?>
<!DOCTYPE html>
<html>
<head>
   <title>This page is for testing the payment proxy script</title>
   <link href='http://fonts.googleapis.com/css?family=Roboto' rel='stylesheet' type='text/css'>
   <style>
      body {
         font-family: "Roboto", sans-serif;
         width: 600px;
         margin: 2rem auto;
      }
      #paymentProxyForm div {
         padding: 0.3rem 1rem;
      }
      #paymentProxyForm label {
         display: inline-block;
         min-width: 150px;
      }
   </style>
   <script src="//code.jquery.com/jquery-2.1.4.min.js" type="text/javascript"></script>
</head>
<body>



<?
   // Step 3: Get PaymentForm JS 
   // Sign Nonce, request payment.js
   $signedNonce = hash("sha256", $nonce.$shared_secret);
   //echo $u.":".$t."\n";
   //echo $nonce.":".$signedNonce."\n";
   $post_array = array("clientid" => $clientId,
      "nonce" => $nonce,
      "signednonce" => $signedNonce,
      "divfield" => "paymentSection",
      "userid" => $u,
      "usertoken" => $t);
   echo "3. POST to PaymentProxy ($paymentProxyUrl.$paymentProxyUri/1.0/getpaymentjs) with Formparams($post_array) \n";
   $script = lib_Util_Service::request("paymentproxy-0.0.2/1.0/getpaymentjs", $paymentProxyUrl, $post_array, true);

   echo $script;
?>
<!-- Step 4: div id match divfield for getpaymentjs call-->
<div id="paymentSection">Default content</div>
<div id="responseStatus" style="width:1300px;"></div>
<div id="responseContent"></div>


<script  type="text/javascript">
(function($) {
   $('#paymentSection').PaymentProxy();
   $(document).on('paymentProxyReturn', function(e) {
      console.log(e.serverData);
      $('#responseStatus').html('<br /><b style="color:#078A00;">Success</b>');
      $('#responseContent').html('<pre>' + JSON.stringify(e.serverData, null, 3) + '</pre>');
   });
 }(jQuery));
</script>
</body>
</html>