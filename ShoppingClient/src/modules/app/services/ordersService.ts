import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class OrdersService {
  constructor(private _http: HttpClient) {}

  public getOrders() {
    return this._http.get<any>(environment.webApiBaseUrl + "orders");
  }
}
