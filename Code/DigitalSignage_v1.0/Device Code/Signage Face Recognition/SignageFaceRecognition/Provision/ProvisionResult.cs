namespace SignageFaceRecognition
{
    class ProvisionResult
    {
        public string registrationId { get; set; }
        public string deviceId { get; set; }
        public string iotHubHostName { get; set; }
        public string etag { get; set; }
        public string createdDateTimeUtc { get; set; }

        public ProvisionStatus registrationStatus { get; set; }
        public Attestation attestation { get; set; }
        public string lastUpdatedDateTimeUtc { get; set; }
        public override string ToString()
        {
            return deviceId;
        }
    }
    class ProvisionStatus
    {
        public string registrationId { get; set; }
        public string status { get; set; }
    }
    class Attestation
    {
        public string type { get; set; }
        public Tpm tpm { get; set; }
    }
    class Tpm
    {
        public string endorsementKey { get; set; }
    }

}