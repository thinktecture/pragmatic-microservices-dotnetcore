import { Order } from "./order";

export class NewOrderMessage {
  public static TypeID: string =
    "OrdersService.Messages.NewOrderMessage, OrdersService";
  public TypeID: string =
    "OrdersService.Messages.NewOrderMessage, OrdersService";

  constructor(public UserId: any, public Order: Order) {}
}
