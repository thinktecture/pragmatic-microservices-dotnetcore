import * as restify from "restify";

export default class StatusController {
  public get(req: restify.Request, res: restify.Response, next: restify.Next) {
    res.send(200, "OK");
  }
}
