extern "C" {
    void GetAPIKey(char* apiKey, int bufferSize)
    {
        NSString* plistFile = [[NSBundle mainBundle] pathForResource:@"SixDegreesSDK" ofType:@"plist"];
        if (plistFile)
        {
            NSDictionary *plistDict = [NSDictionary dictionaryWithContentsOfFile:plistFile];
            if (plistDict)
            {
                id dictApiKey = [plistDict valueForKey:@"SIXDEGREES_API_KEY"];
                if (dictApiKey && [dictApiKey isKindOfClass:[NSString class]])
                {
                    strcpy(apiKey, [dictApiKey UTF8String]);
                }
            }
        }
    }
}
