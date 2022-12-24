export class PartyResultModel {
  public highestDamage!: number
  public highestHeal!: number
  public members!: Array<PartyResultMemberModel> 
}

export class PartyResultMemberModel {
  public key!: string
  public value!: PartyResultMemberDataModel
}

export class PartyResultMemberDataModel {
  public name!: string
  public guild!: string
  public alliance!: string
  public lastUpdate!: number
  public damage!: number
  public heal!: number
  public userGuid!: string
  public interactGuid!: string
  public objectId!: number
  public objectType!: number
  public objectSubType!: number
}
