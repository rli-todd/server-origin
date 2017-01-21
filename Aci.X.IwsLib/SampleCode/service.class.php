<xml></xml>
<?php
include_once('inc/curl.inc.php');

/**
 * This class communicates and interprets the responses from an internal service
 * that returns json.
 *
 *
 */
class lib_Util_Service
{
   /**
    * Executes a curl request to a local service and decodes the json repsonse
    *
    * @param String $endpoint
    * @param String $serviceUrl
    * @param array(String => String) $postArray
    * @param array(String) $postArrayFilterKeys Keys to filter out from $postArray when logging
    * @param array() $rawResponse By reference output variable containing the raw curl response
    * @throws Exception
    * @return array
    */
   public static function request($endpoint, $serviceUrl, $postArray, $isNotJson = false, $postArrayFilterKeys = array(), &$rawResponse = array(), $curlOptions = array())
   {
      // Set a flag indicating we should create the logging post array if necessary
      $filterPostArray = false;
      $postArrayForLogging = array();
      if (is_array($postArrayFilterKeys) && !empty($postArrayFilterKeys) && is_array($postArray) && !empty($postArray))
      {
         $filterPostArray = true;
      }

      Lib_Timer_Timer::start("Beginning curl request");
      $url = $serviceUrl . $endpoint;
      $curl = new Curl_Request($url);

      // set curl option if exists
      foreach ($curlOptions as $field => $value)
      {
         $curl->setOption($field, $value);
      }

      foreach ($postArray as $field => $value)
      {
         $curl->setPostField($field, $value);

         // If we should, create a filtered post array for logging
         if ($filterPostArray)
         {
            $postArrayForLogging[$field] = !in_array($field, $postArrayFilterKeys) ? $value : 'FILTERED_VALUE';
         }
         else
         {
            $postArrayForLogging[$field] = $value;
         }
      }

      try
      {
         $result = $curl->exec();

      }
      catch (Exception $err)
      {
         throw $err;
      }
      $rawResponse = $result->content;

      if (!$isNotJson)
      {
         $output = json_decode($rawResponse, true);
      }
      else
      {
         $output = $rawResponse;
      }

      return $output;

   } // request()

} // lib_Util_Service

?>
