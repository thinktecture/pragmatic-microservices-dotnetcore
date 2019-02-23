import { Config } from "./config";

export let settings: Config = {
  name: "shipping-service",
  version: "1.0.0",
  port: +process.env.PORT || 3000
};
