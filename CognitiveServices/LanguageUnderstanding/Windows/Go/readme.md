	CLI INSTRUCTIONS

	You can use the authoring key instead of the endpoint key. 
	The authoring key allows 1000 endpoint queries a month.


	build endpoint.go from command line
	> go build endpoint.go

	run endpoint from command line for your own app and utterance
	> endpoint -appID xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx -endpointKey xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx -utterance "turn on the lights" -region westus

	run endpoint from command line for the IoT app and utterance
	> endpoint -endpointKey xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx -region westus


	output

	appID has value be402ffc-57f4-4e1f-9c1d-f0d9fa520aa4
	endpointKey has value xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
	region has value westus
	utterance has value turn on the lights
	{
	"query": "turn on the lights",
	"topScoringIntent": {
		"intent": "Utilities.Stop",
		"score": 0.457045376
	},
	"entities": []
	}
