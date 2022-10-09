<template>
  <div class="home">
    <v-container fluid>
      <v-col md="12">
        <v-row justify="center">
        </v-row>
      </v-col>
      <v-col md="12">
        <v-row justify="center">
          <v-col v-col md="12">
            <line-chart
              xtitle="DPS"
              ytitle="Damage ( 3 min )"
              data="/api/combat/damage"
              :precision="3"
              :download="{ background: '#fff' }"
              :refresh="3"
            ></line-chart>
          </v-col>
          <v-col v-col md="12">
            <v-window v-model="window" class="elevation-2" vertical>
              <v-window-item>
                <v-card flat>
                  <v-card-text>
                    {{ this.clusterData }}
                  </v-card-text>
                </v-card>
              </v-window-item>
            </v-window>
          </v-col>
          <v-col v-col md="12">
            <bar-chart data="/api/combat/totalDamage" :download="true" :refresh="3"></bar-chart>
          </v-col>
        </v-row>
      </v-col>
    </v-container>
  </div>
</template>

<style lang="scss">
.home {
  height: 100vh;
}
</style>

<script lang="ts">
import { NavigationGuardNext, Route } from 'vue-router/types/router'
import { Component, Vue, Watch } from 'vue-property-decorator'
import { ClusterResultModel } from '@/models/ClusterResultModel'
import { PartyResultModel } from '@/models/PartyResultModel'

@Component({
  name: 'home',
  components: {
  },
  beforeRouteEnter(to: Route, from: Route, next: NavigationGuardNext<Vue>): void {
    next((vm) => {
      window.scrollTo(0, 0)
      const page = vm as HomeView
      //setInterval(() => {
        page.load()
      //}, 1000);
    })
  }
})
export default class HomeView extends Vue {
  protected isLoading = true
  protected clusterData: ClusterResultModel = new ClusterResultModel
  protected partyData: PartyResultModel = new PartyResultModel

  protected async load(): Promise<void> {
    this.isLoading = true
    try {
      const responseCluster = await this.$axios.get<ClusterResultModel>(`/api/cluster`)
      this.clusterData = responseCluster.data

      const responseParty = await this.$axios.get<PartyResultModel>(`/api/party`)
      this.partyData = responseParty.data
      
      this.isLoading = false
    } catch (e) {
      console.log(e)
    }
  }
}
</script>
