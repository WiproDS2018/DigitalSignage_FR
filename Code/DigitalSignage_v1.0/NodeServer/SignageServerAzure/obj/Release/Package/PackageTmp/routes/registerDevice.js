'use strict';
const express = require('express');
const router = express.Router();
var { iot } = require('../config/config');
var iothub = require('azure-iothub');
var provisioningServiceClient = require('azure-iot-provisioning-service').ProvisioningServiceClient;


var serviceClient = provisioningServiceClient.fromConnectionString(iot.dpsUri);
router.get('/', (req, res) => {

    let id = req.query.id;
    serviceClient.getIndividualEnrollment(id, (err, enrollment) => {
        if (err) {
            res.status(404).send("error");
            return;
        }
        if (enrollment) {
            res.send(enrollment);
        } else res.status(400).send("error");
    });
});

router.post('/', (req, res) => {

    let enrollment = {
        deviceId: req.body.id,
        registrationId: req.body.id,
        iotHubHostName: iot.hubUri,
        attestation: {
            type: 'tpm',
            tpm: {
                endorsementKey: req.body.key
            }
        }
    };


    serviceClient.createOrUpdateIndividualEnrollment(enrollment, function (err, enrollmentResponse) {
        if (err) {
            console.log('error creating the individual enrollment: ' + err);
            res.status(400).send("error");
        } else {
            console.log("enrollment record returned: " + JSON.stringify(enrollmentResponse, null, 2));
            res.send(enrollmentResponse);
        }
    });
    //}
});

router.post("/deviceDetails", (req, res) => {
    console.log(req.body.id, req.body.password);
    if (req.body.password !== "cre@teDev!ce") res.status(400).send();
    else {
        var connectionString = process.env.iotHubConnection;

        var registry = iothub.Registry.fromConnectionString(connectionString);
        var device = {
            deviceId: req.body.id
        }
        registry.create(device, function (err, deviceInfo, deviceRes) {
            if (err) {
                registry.get(device.deviceId, (err, deviceDetails, response) => {
                    res.send({
                        DeviceKey: deviceDetails.authentication.SymmetricKey.primaryKey,
                        IotHub: process.env.iotHubUri
                    })
                });
            }
            if (deviceInfo) {
                res.send({
                    DeviceKey: deviceInfo.authentication.SymmetricKey.primaryKey,
                    IotHub: process.env.iotHubUri
                })
            }
        });
    }

});


module.exports = router;