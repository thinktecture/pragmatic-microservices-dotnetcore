import * as Consul from "consul";
import RegisterOptions = Consul.Agent.Service.RegisterOptions;
import Service = Consul.Agent.Service;

import { IBusConfig, IConsumerDispose, RabbitHutch } from "easynodeq";
import { NewOrderMessage } from "./messages/newOrderMessage";
import { ShippingCreatedMessage } from "./messages/shippingCreatedMessage";
import * as uuid from "node-uuid";

import * as restify from "restify";
import * as corsMiddleware from "restify-cors-middleware";
import StatusController from "./controllers/statusController";
import { settings } from "./config/settings";

var nodeCleanup = require("node-cleanup");

// Consul service agent interaction
let serviceId = "shipping-service-v2-final-01";

nodeCleanup(function(exitCode, signal) {
  console.log("Deregistering with Consul...");
  service.deregister(serviceId, err => {});
});

let registerOptions: RegisterOptions = {
  name: "shipping-service",
  id: serviceId,
  address: "http://localhost",
  port: settings.port,
  check: {
    http: "http://localhost:" + settings.port + "/api/ping",
    interval: "30s"
  }
};

let consul: Consul.Consul = new Consul();
let service: Consul.Agent.Service = consul.agent.service;

console.log("Registering with Consul...");
service.register(registerOptions, data => {
  if (data) {
    console.log("From Consul: " + data);
  }
});

// RabbitMQ subscriber
let busConfig: IBusConfig = {
  heartbeat: 5,
  prefetch: 50,
  rpcTimeout: 10000,
  url: "amqp://localhost:5672",
  vhost: ""
};

let bus = RabbitHutch.CreateBus(busConfig);
bus.Subscribe(NewOrderMessage, "shipping", (message: NewOrderMessage) => {
  console.log("#Got an Order message:");
  console.log(message);

  setTimeout(() => {
    var messageId = uuid.v4();

    bus
      .Publish(
        new ShippingCreatedMessage(
          messageId,
          new Date(),
          message.Order.Id,
          message.UserId
        )
      )
      .then(success =>
        console.log(
          `#Message ${messageId} was ${success ? "" : "not "}published`
        )
      );
  }, 5000);
});

// Restify Web API
export let server = restify.createServer({
  name: settings.name
});

const cors = corsMiddleware({
  origins: ["*"],
  allowHeaders: ["*"]
});

server.pre(cors.preflight);
server.use(cors.actual);

server.get("/api/ping", new StatusController().get);

server.listen(settings.port, function() {
  console.log("Shipping Service running - listening at %s", server.url);
});
