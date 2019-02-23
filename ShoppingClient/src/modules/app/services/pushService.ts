import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { BehaviorSubject } from "rxjs";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@aspnet/signalr";
import { OAuthService } from "angular-oauth2-oidc";

@Injectable()
export class PushService {
  private _hubConnection: HubConnection;

  public orderShipping: BehaviorSubject<string> = new BehaviorSubject(null);
  public orderCreated: BehaviorSubject<string> = new BehaviorSubject(null);

  constructor(private _securityService: OAuthService) {}

  public start(): void {
    this._hubConnection = new HubConnectionBuilder()
    .withUrl(
      environment.signalRBaseUrl +
        "ordersHub" +
        "?authorization=" +
        this._securityService.getAccessToken()
    )
    .configureLogging(LogLevel.Information)
    .build();

  this._hubConnection.on("orderCreated", () => {
    this.orderCreated.next(null);
  });

  this._hubConnection.on("shippingCreated", orderId => {
    this.orderShipping.next(orderId);
  });

  this._hubConnection
    .start()
    .then(() => console.log("SignalR connection established."))
    .catch(err =>
      console.error("SignalR connection not established. " + err)
    );
  }

  public stop(): void {
    if (this._hubConnection) {
      this._hubConnection.stop();
    }

    this._hubConnection = undefined;
  }
}
