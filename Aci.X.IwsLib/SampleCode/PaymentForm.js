﻿<script>/**
 * 
 */
var stateObject = {"AL":"Alabama","AK":"Alaska","AS":"American Samoa","AZ":"Arizona","AR":"Arkansas","CA":"California","CO":"Colorado","CT":"Connecticut","DE":"Delaware","DC":"District Of Columbia","FM":"Federated States Of Micronesia","FL":"Florida","GA":"Georgia","GU":"Guam","HI":"Hawaii","ID":"Idaho","IL":"Illinois","IN":"Indiana","IA":"Iowa","KS":"Kansas","KY":"Kentucky","LA":"Louisiana","ME":"Maine","MH":"Marshall Islands","MD":"Maryland","MA":"Massachusetts","MI":"Michigan","MN":"Minnesota","MS":"Mississippi","MO":"Missouri","MT":"Montana","NE":"Nebraska","NV":"Nevada","NH":"New Hampshire","NJ":"New Jersey","NM":"New Mexico","NY":"New York","NC":"North Carolina","ND":"North Dakota","MP":"Northern Mariana Islands","OH":"Ohio","OK":"Oklahoma","OR":"Oregon","PW":"Palau","PA":"Pennsylvania","PR":"Puerto Rico","RI":"Rhode Island","SC":"South Carolina","SD":"South Dakota","TN":"Tennessee","TX":"Texas","UT":"Utah","VT":"Vermont","VI":"Virgin Islands","VA":"Virginia","WA":"Washington","WV":"West Virginia","WI":"Wisconsin","WY":"Wyoming"};
var provinceObject = {"AB":"Alberta","BC":"British Columbia","MB":"Manitoba","NB":"New Brunswick","NL":"Newfoundland and Labrador","NS":"Nova Scotia","ON":"Ontario","PE":"Prince Edward Island","QC":"Quebec","SK":"Saskatchewan","NT":"Northwest Territories","NU":"Nunavut","YT":"Yukon"};
(function($) {
  $.fn.PaymentProxy = function() {
    var paymentProxy = {};
    paymentProxy.today = new Date(); /* Build the form */
    paymentProxy.formStructure = "<div id='paymentProxyErrors'></div>";
    paymentProxy.formStructure += "<form id='paymentProxyForm'>" +
        "<div class='cc-name'><label for='ccname'>Name on Card</label> <input type='text' id='ccname' name='ccname' /></div>" +
        "<div class='cc-num'><label for='ccnum'>Credit Card Number</label> <input type='text' maxlength='16' id='ccnum' name='ccnum' /></div>" +
        "<div class='cc-cvv'><label for='cccvv'>CVV Number</label> <input type='text' maxlength='4' id='cccvv' name='cccvv' /></div>" +
        "<div class='cc-exp'><label for='ccexpmonth'>Expiry Date</label>" +
        "<select id='ccexpmonth' name='ccexpmonth'>" + "<option value='01'>01 - Jan</option>" +
        "<option value='02'>02 - Feb</option>" + "<option value='03'>03 - Mar</option>" +
        "<option value='04'>04 - Apr</option>" + "<option value='05'>05 - May</option>" +
        "<option value='06'>06 - Jun</option>" + "<option value='07'>07 - Jul</option>" +
        "<option value='08'>08 - Aug</option>" + "<option value='09'>09 - Sep</option>" +
        "<option value='10'>10 - Oct</option>" + "<option value='11'>11 - Nov</option>" +
        "<option value='12'>12 - Dec</option>" + "</select>" + "<select id='ccexpyear' name='ccexpyear'>";
    for (i = 0; i < 14; i++) {
      paymentProxy.formStructure += "<option value='" + (paymentProxy.today.getFullYear() + i) + "'>" + (
          paymentProxy.today.getFullYear() + i) + "</option>";
    }
    paymentProxy.formStructure += "</select></div>";
    paymentProxy.formStructure +=
        "<div class='cc-country'><label for='cccountry'>Country</label>"+
          "<select id='country' name='country'>" + 
            "<option value='us'>United States</option>" + "<option value='ca'>Canada</option>" + "<option value='af'>Afghanistan</option>" +
            "<option value='ax'>Aland Islands</option>" + "<option value='al'>Albania</option>" + "<option value='dz'>Algeria</option>" +
            "<option value='as'>American Samoa</option>" + "<option value='ad'>Andorra</option>" + "<option value='ao'>Angola</option>" +
            "<option value='ai'>Anguilla</option>" + "<option value='aq'>Antarctica</option>" + "<option value='ag'>Antigua & Barbuda</option>" +
            "<option value='ar'>Argentina</option>" + "<option value='am'>Armenia</option>" + "<option value='aw'>Aruba</option>" +
            "<option value='au'>Australia</option>" + "<option value='at'>Austria</option>" + "<option value='az'>Azerbaijan</option>" +
            "<option value='bs'>Bahamas</option>" + "<option value='bh'>Bahrain</option>" + "<option value='bd'>Bangladesh</option>" +
            "<option value='bb'>Barbados</option>" + "<option value='by'>Belarus</option>" + "<option value='be'>Belgium</option>" +
            "<option value='bz'>Belize</option>" + "<option value='bj'>Benin</option>" + "<option value='bm'>Bermuda</option>" +
            "<option value='bt'>Bhutan</option>" + "<option value='bo'>Bolivia</option>" + "<option value='ba'>Bosnia & Herzegovina</option>" +
            "<option value='bw'>Botswana</option>" + "<option value='bv'>Bouvet Island</option>" + "<option value='br'>Brazil</option>" +
            "<option value='io'>British Indian Ocean Territory</option>" + "<option value='bn'>Brunei</option>" + "<option value='bg'>Bulgaria</option>" +
            "<option value='bf'>Burkina Faso</option>" + "<option value='bi'>Burundi</option>" + "<option value='kh'>Cambodia</option>" +
            "<option value='cm'>Cameroon</option>" + "<option value='cv'>Cape Verde</option>" + "<option value='ky'>Cayman Islands</option>" +
            "<option value='cf'>Central African Republic</option>" + "<option value='td'>Chad</option>" + "<option value='cl'>Chile</option>" +
            "<option value='cn'>China</option>" + "<option value='cx'>Christmas Island</option>" + "<option value='cc'>Cocos (Keeling) Islands</option>" +
            "<option value='co'>Colombia</option>" + "<option value='km'>Comoros</option>" + "<option value='cg'>Congo</option>" +
            "<option value='cd'>Congo, Democratic Republic of the</option>" + "<option value='ck'>Cook Islands</option>" + "<option value='cr'>Costa Rica</option>" +
            "<option value='hr'>Croatia</option>" + "<option value='cu'>Cuba</option>" + "<option value='cy'>Cyprus</option>" +
            "<option value='cz'>Czech Republic</option>" + "<option value='dk'>Denmark</option>" + "<option value='dj'>Djibouti</option>" +
            "<option value='dm'>Dominica</option>" + "<option value='do'>Dominican Republic</option>" + "<option value='tl'>East Timor</option>" +
            "<option value='ec'>Ecuador</option>" + "<option value='eg'>Egypt</option>" + "<option value='sv'>El Salvador</option>" +
            "<option value='gq'>Equatorial Guinea</option>" + "<option value='er'>Eritrea</option>" + "<option value='ee'>Estonia</option>" +
            "<option value='et'>Ethiopia</option>" + "<option value='fk'>Falkland Islands</option>" + "<option value='fo'>Faroe Islands</option>" +
            "<option value='fj'>Fiji</option>" + "<option value='fi'>Finland</option>" + "<option value='fr'>France</option>" +
            "<option value='gf'>French Guiana</option>" + "<option value='pf'>French Polynesia</option>" + "<option value='tf'>French Southern Territories</option>" +
            "<option value='ga'>Gabon</option>" + "<option value='gm'>Gambia</option>" + "<option value='ge'>Georgia</option>" +
            "<option value='de'>Germany</option>" + "<option value='gh'>Ghana</option>" + "<option value='gi'>Gibraltar</option>" +
            "<option value='gr'>Greece</option>" + "<option value='gl'>Greenland</option>" + "<option value='gd'>Grenada</option>" +
            "<option value='gp'>Guadeloupe</option>" + "<option value='gu'>Guam</option>" + "<option value='gt'>Guatemala</option>" +
            "<option value='gg'>Guernsey</option>" + "<option value='gn'>Guinea</option>" + "<option value='gw'>Guinea-Bissau</option>" +
            "<option value='gy'>Guyana</option>" + "<option value='ht'>Haiti</option>" + "<option value='hm'>Heard & McDonald Islands</option>" +
            "<option value='hn'>Honduras</option>" + "<option value='hk'>Hong Kong</option>" + "<option value='hu'>Hungary</option>" +
            "<option value='is'>Iceland</option>" + "<option value='in'>India</option>" + "<option value='id'>Indonesia</option>" +
            "<option value='ir'>Iran</option>" + "<option value='iq'>Iraq</option>" + "<option value='ie'>Ireland</option>" +
            "<option value='im'>Isle of Man</option>" + "<option value='il'>Israel</option>" + "<option value='it'>Italy</option>" +
            "<option value='ci'>Ivory Coast</option>" + "<option value='jm'>Jamaica</option>" + "<option value='jp'>Japan</option>" +
            "<option value='je'>Jersey</option>" + "<option value='jo'>Jordan</option>" + "<option value='kz'>Kazakhstan</option>" +
            "<option value='ke'>Kenya</option>" + "<option value='ki'>Kiribati</option>" + "<option value='ks'>Kosovo</option>" +
            "<option value='kw'>Kuwait</option>" + "<option value='kg'>Kyrgyzstan</option>" + "<option value='la'>Laos</option>" +
            "<option value='lv'>Latvia</option>" + "<option value='lb'>Lebanon</option>" + "<option value='ls'>Lesotho</option>" +
            "<option value='lr'>Liberia</option>" + "<option value='ly'>Libya</option>" + "<option value='li'>Liechtenstein</option>" +
            "<option value='lt'>Lithuania</option>" + "<option value='lu'>Luxembourg</option>" + "<option value='mo'>Macau</option>" +
            "<option value='mk'>Macedonia</option>" + "<option value='mg'>Madagascar</option>" + "<option value='mw'>Malawi</option>" +
            "<option value='my'>Malaysia</option>" + "<option value='mv'>Maldives</option>" + "<option value='ml'>Mali</option>" +
            "<option value='mt'>Malta</option>" + "<option value='mh'>Marshall Islands</option>" + "<option value='mq'>Martinique</option>" +
            "<option value='mr'>Mauritania</option>" + "<option value='mu'>Mauritius</option>" + "<option value='yt'>Mayotte</option>" +
            "<option value='mx'>Mexico</option>" + "<option value='fm'>Micronesia</option>" + "<option value='md'>Moldova</option>" +
            "<option value='mc'>Monaco</option>" + "<option value='mn'>Mongolia</option>" + "<option value='me'>Montenegro</option>" +
            "<option value='ms'>Montserrat</option>" + "<option value='ma'>Morocco</option>" + "<option value='mz'>Mozambique</option>" +
            "<option value='mm'>Myanmar</option>" + "<option value='na'>Namibia</option>" + "<option value='nr'>Nauru</option>" +
            "<option value='np'>Nepal</option>" + "<option value='nl'>Netherlands</option>" + "<option value='an'>Netherlands Antilles</option>" +
            "<option value='nc'>New Caledonia</option>" + "<option value='nz'>New Zealand</option>" + "<option value='ni'>Nicaragua</option>" +
            "<option value='ne'>Niger</option>" + "<option value='ng'>Nigeria</option>" + "<option value='nu'>Niue</option>" +
            "<option value='nf'>Norfolk Island</option>" + "<option value='kp'>North Korea</option>" + "<option value='mp'>Northern Mariana Islands</option>" +
            "<option value='no'>Norway</option>" + "<option value='om'>Oman</option>" + "<option value='pk'>Pakistan</option>" +
            "<option value='pw'>Palau</option>" + "<option value='ps'>Palestine</option>" + "<option value='pa'>Panama</option>" +
            "<option value='pg'>Papua New Guinea</option>" + "<option value='py'>Paraguay</option>" + "<option value='pe'>Peru</option>" +
            "<option value='ph'>Philippines</option>" + "<option value='pn'>Pitcairn Island</option>" + "<option value='pl'>Poland</option>" +
            "<option value='pt'>Portugal</option>" + "<option value='pr'>Puerto Rico</option>" + "<option value='qa'>Qatar</option>" +
            "<option value='re'>Reunion Island</option>" + "<option value='ro'>Romania</option>" + "<option value='ru'>Russia</option>" +
            "<option value='rw'>Rwanda</option>" + "<option value='bl'>Saint Barthelemy</option>" + "<option value='sh'>Saint Helena</option>" +
            "<option value='kn'>Saint Kitts & Nevis</option>" + "<option value='lc'>Saint Lucia</option>" + "<option value='mf'>Saint Martin</option>" +
            "<option value='pm'>Saint Pierre & Miquelon</option>" + "<option value='vc'>Saint Vincent & the Grenadines</option>" + "<option value='ws'>Samoa</option>" +
            "<option value='sm'>San Marino</option>" + "<option value='st'>Sao Tome & Principe</option>" + "<option value='sa'>Saudi Arabia</option>" +
            "<option value='sn'>Senegal</option>" + "<option value='rs'>Serbia</option>" + "<option value='sc'>Seychelles</option>" +
            "<option value='sl'>Sierra Leone</option>" + "<option value='sg'>Singapore</option>" + "<option value='sk'>Slovakia</option>" +
            "<option value='si'>Slovenia</option>" + "<option value='sb'>Solomon Islands</option>" + "<option value='so'>Somalia</option>" +
            "<option value='za'>South Africa</option>" + "<option value='gs'>South Georgia & S. Sandwich Islands</option>" + "<option value='kr'>South Korea</option>" +
            "<option value='es'>Spain</option>" + "<option value='lk'>Sri Lanka</option>" + "<option value='sd'>Sudan</option>" +
            "<option value='sr'>Suriname</option>" + "<option value='sj'>Svalbard & Jan Mayen</option>" + "<option value='sz'>Swaziland</option>" +
            "<option value='se'>Sweden</option>" + "<option value='ch'>Switzerland</option>" + "<option value='sy'>Syria</option>" +
            "<option value='tw'>Taiwan</option>" + "<option value='tj'>Tajikistan</option>" + "<option value='tz'>Tanzania</option>" +
            "<option value='th'>Thailand</option>" + "<option value='tg'>Togo</option>" + "<option value='tk'>Tokelau</option>" +
            "<option value='to'>Tonga</option>" + "<option value='tt'>Trinidad & Tobago</option>" + "<option value='tn'>Tunisia</option>" +
            "<option value='tr'>Turkey</option>" + "<option value='tm'>Turkmenistan</option>" + "<option value='tc'>Turks & Caicos Islands</option>" +
            "<option value='tv'>Tuvalu</option>" + "<option value='ug'>Uganda</option>" + "<option value='ua'>Ukraine</option>" +
            "<option value='ae'>United Arab Emirates</option>" + "<option value='uk'>United Kingdom</option>" + "<option value='um'>US Minor Outlying Islands</option>" +
            "<option value='uy'>Uruguay</option>" + "<option value='uz'>Uzbekistan</option>" + "<option value='vu'>Vanuatu</option>" +
            "<option value='va'>Vatican City</option>" + "<option value='ve'>Venezuela</option>" + "<option value='vn'>Vietnam</option>" +
            "<option value='vg'>Virgin Islands (UK)</option>" + "<option value='vi'>Virgin Islands (USA)</option>" + "<option value='wf'>Wallis & Futuna Islands</option>" +
            "<option value='eh'>Western Sahara</option>" + "<option value='ye'>Yemen</option>" + "<option value='zm'>Zambia</option>" + "<option value='zw'>Zimbabwe</option>" +
          "</select>"+
        "</div>" +
        "<div class='cc-address'><label for='ccaddress'>Billing Address</label> <input type='text' id='address' name='address' /></div>" +
        "<div class='cc-city'><label for='cccity'>City</label> <input type='text' id='city' name='city' /></div>" +
        "<div class='cc-state'><label for='ccstate'>State</label> " +
           "<select id='state' name='state'>";
    for (var st in stateObject) {
      if (stateObject.hasOwnProperty(st)) {
        paymentProxy.formStructure += "<option value='"+st+"'>"+stateObject[st]+"</option>"
      }
    }
    paymentProxy.formStructure += 
      "</select></div>" +
        "<div class='cc-zip'><label for='cczip'>Zip</label> <input type='text' id='cczip' name='cczip' /></div>";
    paymentProxy.formStructure +=
        "<div class='cc-submit'><button type='submit' id='ccsubmit' name='ccsubmit'>Submit</button></div>";

    paymentProxy.formStructure += "<input type='hidden' id='user_id' name='user_id' value='1243' />" +
        "<input type='hidden' id='user_token' name='user_token' value='4adc44e0-6e65-4d4c-9c4c-32dbcf514ace' />" +
        "<input type='hidden' id='client_id' name='client_id' value='800ci-9f9b1a97-4dc9-4f38-a4d4-644a9bfc1eb3' />" +
        "<input type='hidden' id='timestamp' name='timestamp' value='2015-09-20 07:43:22' />" +
        "<input type='hidden' id='submittimestamp' name='submittimestamp' value='' />" +
        "<input type='hidden' id='nonce' name='nonce' value='5260693c-087f-4334-b266-18a5d1a25ddf' />" +
        "<input type='hidden' id='signednonce' name='signednonce' value='b4e1a9f8886f934ae49966fd8cda6ad17d7d77725cdaeb6c054c79ac4185b14b' />";

    paymentProxy.formStructure += "</form>";
    /* Draw the form */
    $(this).html(paymentProxy.formStructure);
    /* Some AJAX for fun and profit */
    $(document).on('submit', '#paymentProxyForm', function(e) {
      e.preventDefault();
      $('#submittimestamp').val(new Date());
      $.ajax({
        type: "POST",
        url: 'http://integ-jweb11.tuk2.intelius.com:8080/paymentproxy-0.0.2/1.0/acceptformresponse',
        data: $(this).serialize()
      }).done(function(data) {
        $.event.trigger({
          type: 'paymentProxyReturn',
          serverData: jQuery.parseJSON(data),
          time: paymentProxy.today
        });
      }).fail(function(data) {
        $("#paymentProxyErrors").html(data);
      });
    });
    /* Change out the "State" values when country is changed */
    $(document).on('change', '#country', function() {
      var country = $(this).val();
      if (country == 'us') {
        $('label[for="ccstate"]').html('State');
        var stateOptions = "";
        for (var st in stateObject) {
          if (stateObject.hasOwnProperty(st)) {
            stateOptions += "<option value='"+st+"'>"+stateObject[st]+"</option>"
          }
        }
        $('#state').html(stateOptions);
      }
      else if (country == 'ca') {
        $('label[for="ccstate"]').html('Province');
        var provinceOptions = "";
        for (var pv in provinceObject) {
          if (provinceObject.hasOwnProperty(pv)) {
            provinceOptions += "<option value='"+pv+"'>"+provinceObject[pv]+"</option>"
          }
        }
        $('#state').html(provinceOptions);
      }
      else {
        $('label[for="ccstate"]').html('State');
        $('select#state').html('<option value="oo">Outside U.S./Canada</option>');
      }
    });
  };
}(jQuery));</script>