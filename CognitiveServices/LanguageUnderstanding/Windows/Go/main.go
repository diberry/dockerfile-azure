package main

/* Do dependencies */
import (
	"fmt"
	"flag"
	"net/http"
	"net/url"
	"io/ioutil"
	"log"
	"os"
)

/* 
	Analyze text

	appID = public app ID = be402ffc-57f4-4e1f-9c1d-f0d9fa520aa4
	endpointKey = Azure Language Understanding key, or Authoring key if it still has quota
	host = endpoint host
	utterance = text to analyze

*/
func endpointPrediction(appID string, endpointKey string, host string, utterance string) {

	var endpointUrl = fmt.Sprintf("%s/%s?subscription-key=%s&verbose=false&q=%s", host, appID, endpointKey, url.QueryEscape(utterance))
	
	response, err := http.Get(endpointUrl)

	// 401 - check value of 'subscription-key' - do not use authoring key!
	if err!=nil {
		// handle error
		fmt.Println("error from Get")
		log.Fatal(err)
	}
	
	response2, err2 := ioutil.ReadAll(response.Body)

	if err2!=nil {
		// handle error
		fmt.Println("error from ReadAll")
		log.Fatal(err2)
	}

	fmt.Println("response")
	fmt.Println(string(response2))
}

func main() {
	
	var appID = flag.String("appID", "df67dcdb-c37d-46af-88e1-8b97951ca1c2", "LUIS appID")
	var endpointKey = flag.String("endpointKey", os.Getenv("LUIS_KEY"), "LUIS endpoint key")
	var host = flag.String("host", os.Getenv("LUIS_HOST"), "LUIS app publish host")
	var utterance = flag.String("utterance", "turn on the bedroom light", "utterance to predict")

	flag.Parse()
	
	fmt.Println("appID has value", *appID)
	fmt.Println("endpointKey has value", *endpointKey)
	fmt.Println("host has value", *host)
	fmt.Println("utterance has value", *utterance)

	endpointPrediction(*appID, *endpointKey, *host, *utterance)

}