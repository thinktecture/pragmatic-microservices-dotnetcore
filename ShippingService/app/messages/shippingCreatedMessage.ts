export class ShippingCreatedMessage {
  public static TypeID: string =
    "OrdersService.Messages.ShippingCreatedMessage, OrdersService";
  public TypeID: string =
    "OrdersService.Messages.ShippingCreatedMessage, OrdersService";

  constructor(
    public Id: any,
    public Created: Date,
    public OrderId: any,
    public UserId: any
  ) {}
}
