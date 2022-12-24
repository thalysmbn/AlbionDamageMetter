import { AvalonTunnelType } from '@/enum/AvalonTunnelType'
import { ClusterMode } from '@/enum/ClusterMode'
import { MapType } from '@/enum/MapType'
import { Tier } from '@/enum/Tier'
import { PlayerGameObject } from './PlayerGameObject'

export class ClusterResultModel {
  public entered!: string
  public end!: string
  public mapType!: MapType
  public guid!: string
  public index!: string
  public instanceName!: string
  public worldMapDataType!: string
  public dungeonInformation!: string
  public mainClusterIndex!: string
  public worldJsonType!: string
  public file!: string
  public uniqueName!: string
  public uniqueClusterName!: string
  public clusterMode!: ClusterMode
  public avalonTunnelType!: AvalonTunnelType
  public tier!: Tier
  public entities!: number
  public combatLog!: number
}
