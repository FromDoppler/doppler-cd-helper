using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.CDHelper.SwarmClient
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Allow properties with camelCase because it represents a JSON")]
    public record SwarmServiceDescription
    {
        public string id { get; init; }

        // TODO: add more properties in order to represent the Swarmpit response:
        //   {
        //     "updatedAt": "string",
        //     "agent": true,
        //     "configs": [
        //       {
        //         "id": "string",
        //         "configName": "string",
        //         "configTarget": "string",
        //         "uid": 0,
        //         "gid": 0,
        //         "mode": 0
        //       }
        //     ],
        //     "command": [
        //       "string"
        //     ],
        //     "hosts": [
        //       {
        //         "name": "string",
        //         "value": "string"
        //       }
        //     ],
        //     "createdAt": "string",
        //     "user": "string",
        //     "containerLabels": [
        //       {
        //         "name": "string",
        //         "value": "string"
        //       }
        //     ],
        //     "logdriver": {
        //       "name": "string",
        //       "opts": [
        //         {
        //           "name": "string",
        //           "value": "string"
        //         }
        //       ]
        //     },
        //     "tty": true,
        //     "repository": {
        //       "name": "string",
        //       "tag": "string",
        //       "image": "string",
        //       "imageDigest": "string"
        //     },
        //     "id": "string",
        //     "networks": [
        //       {
        //         "created": "string",
        //         "serviceAliases": [
        //           "string"
        //         ],
        //         "internal": true,
        //         "attachable": true,
        //         "driver": "string",
        //         "ingress": true,
        //         "id": "string",
        //         "enableIPv6": true,
        //         "labels": {},
        //         "scope": "string",
        //         "ipam": {
        //           "subnet": "string",
        //           "gateway": "string"
        //         },
        //         "networkName": "string",
        //         "options": [
        //           {
        //             "name": "string",
        //             "value": {}
        //           }
        //         ],
        //         "stack": "string"
        //       }
        //     ],
        //     "mode": "string",
        //     "labels": [
        //       {
        //         "name": "string",
        //         "value": "string"
        //       }
        //     ],
        //     "mounts": [
        //       {
        //         "containerPath": "string",
        //         "host": "string",
        //         "type": "string",
        //         "id": "string",
        //         "volumeOptions": {
        //           "labels": {},
        //           "driver": {
        //             "name": "string",
        //             "options": {}
        //           }
        //         },
        //         "readOnly": true,
        //         "stack": "string"
        //       }
        //     ],
        //     "status": {
        //       "tasks": {
        //         "running": 0,
        //         "total": 0
        //       },
        //       "update": "string",
        //       "message": "string"
        //     },
        //     "serviceName": "string",
        //     "deployment": {
        //       "update": {
        //         "parallelism": 0,
        //         "delay": 0,
        //         "order": "string",
        //         "failureAction": "string"
        //       },
        //       "forceUpdate": 0,
        //       "restartPolicy": {
        //         "condition": "string",
        //         "delay": 0,
        //         "window": 0,
        //         "attempts": 0
        //       },
        //       "rollback": {
        //         "parallelism": 0,
        //         "delay": 0,
        //         "order": "string",
        //         "failureAction": "string"
        //       },
        //       "rollbackAllowed": true,
        //       "autoredeploy": true,
        //       "placement": [
        //         {
        //           "rule": "string"
        //         }
        //       ]
        //     },
        //     "healthcheck": {
        //       "test": [
        //         "string"
        //       ],
        //       "interval": 0,
        //       "timeout": 0,
        //       "retries": 0
        //     },
        //     "dir": "string",
        //     "variables": [
        //       {
        //         "name": "string",
        //         "value": "string"
        //       }
        //     ],
        //     "links": [
        //       {
        //         "name": "string",
        //         "value": "string"
        //       }
        //     ],
        //     "state": "string",
        //     "version": 0,
        //     "replicas": 0,
        //     "secrets": [
        //       {
        //         "id": "string",
        //         "secretName": "string",
        //         "secretTarget": "string",
        //         "uid": 0,
        //         "gid": 0,
        //         "mode": 0
        //       }
        //     ],
        //     "resources": {
        //       "reservation": {
        //         "cpu": 0,
        //         "memory": 0
        //       },
        //       "limit": {
        //         "cpu": 0,
        //         "memory": 0
        //       }
        //     },
        //     "ports": [
        //       {
        //         "containerPort": 0,
        //         "mode": "string",
        //         "protocol": "string",
        //         "hostPort": 0
        //       }
        //     ],
        //     "stack": "string"
        //   }
    }
}
