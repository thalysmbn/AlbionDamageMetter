<template>
  <div class="home">
    <v-container fluid>
      <v-col md="12">
        <v-row justify="center">
          <v-col v-col md="12">
            <v-card class="mx-auto" tile>
              <v-list-item two-line>
                <v-list-item-content>
                  <v-list-item-title>Entered</v-list-item-title>
                  <v-list-item-subtitle>{{
                    GetHumanDate(this.clusterData.entered)
                  }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>End</v-list-item-title>
                  <v-list-item-subtitle>{{
                    GetHumanDate(this.clusterData.end)
                  }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>Mode</v-list-item-title>
                  <v-list-item-subtitle>{{ this.clusterData.clusterHistoryString1 }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>Name</v-list-item-title>
                  <v-list-item-subtitle>{{ this.clusterData.clusterHistoryString2 }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>Type</v-list-item-title>
                  <v-list-item-subtitle>{{ this.clusterData.clusterHistoryString3 }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>Tier</v-list-item-title>
                  <v-list-item-subtitle>{{ this.clusterData.tier }}</v-list-item-subtitle>
                </v-list-item-content>
                <v-list-item-content>
                  <v-list-item-title>Entities</v-list-item-title>
                  <v-list-item-subtitle>{{ this.clusterData.entities }}</v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </v-card>
          </v-col>
          <v-divider></v-divider>
          <v-col v-col md="12">
            <line-chart
              xtitle="DPS"
              :data="this.clusterData.damageResult"
              :precision="3"
              :download="GetDownload('DPS Graph', this.$route.params.date)"
              :curve="false"
              :points="false"
              :stacked="true"
            ></line-chart>
          </v-col>
          <v-divider></v-divider>
        </v-row>
      </v-col>
      <v-col md="12">
        <v-row justify="center">
          <v-col v-col md="6">
            <bar-chart
              xtitle="Total Damage"
              :data="this.clusterData.totalDamageList"
              :download="GetDownload('Total Damage', this.$route.params.date)"
              :stacked="true"
              :colors="['#dc3912']"
            ></bar-chart>
          </v-col>
          <v-divider></v-divider>
          <v-col v-col md="6">
            <bar-chart
              xtitle="Highest Damage"
              :data="this.clusterData.highestDamageList"
              :download="GetDownload('Highest Damage', this.$route.params.date)"
              :stacked="true"
              :colors="['#dc3912']"
            ></bar-chart>
          </v-col>
          <v-divider></v-divider>
          <v-col v-col md="12">
            <line-chart
              xtitle="Heal"
              :data="this.clusterData.healResult"
              :precision="3"
              :download="GetDownload('Heal Graph', this.$route.params.date)"
              :curve="false"
              :points="false"
              :stacked="true"
            ></line-chart>
          </v-col>
        </v-row>
      </v-col>
      <v-col md="12">
        <v-row justify="center">
          <v-divider></v-divider>
          <v-col v-col md="12">
            <bar-chart
              xtitle="Total Healing"
              :data="this.clusterData.totalHealingList"
              :download="GetDownload('Total Healing', this.$route.params.date)"
              :colors="['#109618']"
              :stacked="true"
            ></bar-chart>
          </v-col>
        </v-row>
      </v-col>
    </v-container>
  </div>
</template>

<style lang="scss">
.home {
  .cluster {
    padding: 0;
    background: rgb(0, 0, 0, 0.35);
  }
}
</style>

<script lang="ts">
import { MapType } from '@/enum/MapType'
import { ClusterMode } from '@/enum/ClusterMode'
import { NavigationGuardNext, Route } from 'vue-router/types/router'
import { Component, Vue, Watch } from 'vue-property-decorator'
import { ClusterResultModel } from '@/models/ClusterResultModel'
import { PartyResultModel } from '@/models/PartyResultModel'

@Component({
  name: 'cluster',
  components: {
  },
  beforeRouteEnter(to: Route, from: Route, next: NavigationGuardNext<Vue>): void {
    next((vm) => {
      window.scrollTo(0, 0)
      const page = vm as ClusterView
      page.load()
    })
  }
})
export default class ClusterView extends Vue {
  protected isLoading = true
  protected clusterData: ClusterResultModel = new ClusterResultModel
  protected partyData: PartyResultModel = new PartyResultModel

  protected async load(): Promise<void> {
    this.isLoading = true
    try {
      const response = await this.$axios.get<ClusterResultModel>(
        `/api/cluster/data/${this.$route.params.date}`
      )
      this.clusterData = response.data

      this.isLoading = false
    } catch (e) {
      console.log(e)
    }
  }

  public GetDownload(extension: string, date: string) {
    return `${extension} - ${date}.png`
  }

  public GetHumanDate(date: string): string {
    var current = new Date(date)
    return current.toUTCString()
  }

  public GetClusterMode(number: number): string {
    return ClusterMode[number] as string
  }
}
</script>
