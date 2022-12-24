<template>
  <div class="home">
    <v-container fluid>
      <v-col md="12">
        <v-row justify="center">
          <v-simple-table fixed-header :items-per-page="5" dense>
            <template v-slot:default>
              <thead>
                <tr>
                  <th class="text-left">Entered</th>
                  <th class="text-left">Mode</th>
                  <th class="text-left">Name</th>
                  <th class="text-left">Type</th>
                  <th class="text-left">Tier</th>
                  <th class="text-left">Map Type</th>
                  <th class="text-left">Entities</th>
                  <th class="text-left">Combat Log</th>
                  <th class="text-left">End</th>
                  <th class="text-left"></th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="item in clusterData" :key="item.entered">
                  <td>{{ item.entered }}</td>
                  <td>{{ item.clusterHistoryString1 }}</td>
                  <td>{{ item.clusterHistoryString2 }}</td>
                  <td>{{ item.clusterHistoryString3 }}</td>
                  <td>{{ item.tier }}</td>
                  <td>{{ GetMapType(item.mapType) }}</td>
                  <td>{{ item.entities }}</td>
                  <td>{{ item.combatLog }}</td>
                  <td>{{ item.end }}</td>
                  <td>
                    <router-link
                      :to="{
                        name: 'Cluster',
                        params: { date: item.entered.replace('Z', '') }
                      }"
                    >
                      <v-icon small>mdi-arrow-right-bold</v-icon>
                    </router-link>
                  </td>
                </tr>
              </tbody>
            </template>
          </v-simple-table>
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
  name: 'clusters',
  components: {
  },
  beforeRouteEnter(to: Route, from: Route, next: NavigationGuardNext<Vue>): void {
    next((vm) => {
      window.scrollTo(0, 0)
      const page = vm as ClustersView
      page.load()
    })
  }
})
export default class ClustersView extends Vue {
  protected isLoading = true
  protected clusterData: ClusterResultModel = new ClusterResultModel
  protected partyData: PartyResultModel = new PartyResultModel

  protected async load(): Promise<void> {
    this.isLoading = true
    try {
      const response = await this.$axios.get<ClusterResultModel>(`/api/cluster/clusters`)
      this.clusterData = response.data

      this.isLoading = false
    } catch (e) {
      console.log(e)
    }
  }

  public GetMapType(number: number): string {
    return MapType[number] as string
  }

  public GetClusterMode(number: number): string {
    return ClusterMode[number] as string
  }
}
</script>
