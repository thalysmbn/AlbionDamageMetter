<template>
  <div class="home">
    <v-container fluid>
      <v-col md="12">
        <v-row justify="center">
          <v-col v-col md="12">
            <line-chart
              xtitle="DPS"
              ytitle="Damage ( 3 min )"
              data="/api/combat/damage"
              :precision="3"
              :curve="false"
              :points="false"
              :stacked="true"
              :refresh="2"
            ></line-chart>
          </v-col>
          <v-col v-col md="6">
            <bar-chart
              xtitle="Highest DPS"
              data="/api/combat/highestDps"
              :download="true"
              :colors="['#dc3912']"
              :refresh="1"
            ></bar-chart>
          </v-col>
          <v-col v-col md="6">
            <bar-chart
              xtitle="Total Damage"
              data="/api/combat/totalDamage"
              :legend="a"
              :download="true"
              :stacked="true"
              :colors="['#dc3912']"
              :refresh="1"
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
      const responseParty = await this.$axios.get<PartyResultModel>(`/api/party`)
      this.partyData = responseParty.data
      
      this.isLoading = false
    } catch (e) {
      console.log(e)
    }
  }
}
</script>
