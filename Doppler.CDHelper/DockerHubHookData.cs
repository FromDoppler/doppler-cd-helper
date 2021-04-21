using System;

namespace Doppler.CDHelper
{
    public record DockerHubHookData
    {
        public string callback_url { get; init; }

        // TODO: add more properties in order to represent the Docker Hub data:
        // {
        //     "push_data": {
        //         "pushed_at": 1618614391,
        //         "images": [],
        //         "tag": "pr-1",
        //         "pusher": "dopplerdock"
        //     },
        //     "callback_url": "https://registry.hub.docker.com/u/dopplerdock/doppler-cd-helper/hook/2jiedged52hb342h3fi133acgdggdi1ih/",
        //     "repository": {
        //         "status": "Active",
        //         "description": "",
        //         "is_trusted": false,
        //         "full_description": null,
        //         "repo_url": "https://hub.docker.com/r/dopplerdock/doppler-cd-helper",
        //         "owner": "dopplerdock",
        //         "is_official": false,
        //         "is_private": true,
        //         "name": "doppler-cd-helper",
        //         "namespace": "dopplerdock",
        //         "star_count": 0,
        //         "comment_count": 0,
        //         "date_created": 1618603558,
        //         "repo_name": "dopplerdock/doppler-cd-helper"
        //     }
        // }
    }
}
