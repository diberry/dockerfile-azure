winpty docker run -it -e PERSONALIZER_KEY='replace-with-your-key' -e PERSONALIZER_ENDPOINT='https://westus2.api.cognitive.microsoft.com/' --mount src="/c/Users/diberry/repos/dockerfile-azure/CognitiveServices/Personalizer/Windows/Python",dst=/usr/src/app,type=bind --rm my-py3-personalizer-image