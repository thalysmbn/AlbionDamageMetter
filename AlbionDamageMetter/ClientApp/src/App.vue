<template>
  <v-app :dark="true">
    <v-navigation-drawer
      persistent
      v-bind:width="325"
      :mini-variant="miniVariant"
      :clipped="clipped"
      v-model="drawer"
      color="#000"
      enable-resize-watcher
      app
      dark
    >
      <v-list>
        <template v-for="(item, i) in items">
          <template v-if="item.target">
            <v-list-item value="true" :key="i" :href="item.link" :target="item.target">
              <v-list-item-action>
                <v-icon v-html="item.icon"></v-icon>
              </v-list-item-action>
              <v-list-item-content>
                <v-list-item-title color="#92ABC2" v-text="item.title"></v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
          <template v-else>
            <v-list-item value="true" :key="i" :to="item.link">
              <v-list-item-action>
                <v-icon v-html="item.icon"></v-icon>
              </v-list-item-action>
              <v-list-item-content>
                <v-list-item-title color="#92ABC2" v-text="item.title"></v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </template>
        <template v-if="isAuthenticated">
          <v-list-item value="true" href="/authentication/signin">
            <v-list-item-action>
              <v-icon>mdi-logout</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title color="#92ABC2" v-text="'Logout'"></v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
        <template v-else> </template>
      </v-list>
    </v-navigation-drawer>
    <v-app-bar app :clipped-left="clipped" dark>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-btn class="d-none d-lg-flex" icon @click.stop="miniVariant = !miniVariant">
        <v-icon v-html="miniVariant ? 'mdi-chevron-right' : 'mdi-chevron-left'"></v-icon>
      </v-btn>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
      <v-tab>
        <v-badge :color="GetClusterColor(clusterData.clusterMode)" :content="clusterData.tier">
          {{ clusterData.clusterHistoryString1 }}
          {{ clusterData.clusterHistoryString2 }}
          {{ clusterData.clusterHistoryString3 }}
        </v-badge>
      </v-tab>
    </v-app-bar>
    <v-main class="main">
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { MapType } from '../src/enum/MapType'
import { ClusterMode } from '../src/enum/ClusterMode'
import './assets/custom.scss'
import { Action, Getter } from 'vuex-class'
import { Component, Vue, Watch } from 'vue-property-decorator'
import { ClusterResultModel } from '@/models/ClusterResultModel'
const namespace = 'account'

@Component({})
export default class App extends Vue {
  protected clusterData: ClusterResultModel = new ClusterResultModel

  protected isLoading = true
  private clipped = true
  private drawer = true
  private miniVariant = true
  private right = true
  private title = 'Albion Arms'
  private items = [
    { title: 'Home', icon: 'mdi-home', link: '/' },
    { title: 'Clusters', icon: 'mdi-sword-cross', link: '/clusters' }
  ]

  @Getter('isAuthenticated', { namespace })
  private isAuthenticated!: boolean

  @Getter('discordId', { namespace })
  private discordId!: boolean

  @Getter('discordAvatarUrl', { namespace })
  private discordAvatarUrl!: boolean

  @Action('setAuthenticated', { namespace })
  private setAuthenticated!: (isAuthenticated: boolean) => void

  @Action('setDiscordId', { namespace })
  private setDiscordId!: (discordId: string) => void

  @Action('setDiscordAvatarUrl', { namespace })
  private setDiscordAvatarUrl!: (discordId: string) => void

  public async created(): Promise<void> {
    setInterval(() => {
      this.load()
    }, 1000)
  }

  public GetMapType(number: number): string {
    return MapType[number] as string
  }

  public GetClusterMode(number: number): string {
    return ClusterMode[number] as string
  }

  public GetClusterColor(mode: ClusterMode): string {
    switch (mode) {
      case ClusterMode.Yellow:
        return 'yellow'
      case ClusterMode.Red:
        return 'red'
      case ClusterMode.Black:
      case ClusterMode.AvalonTunnel:
        return 'black'
      case ClusterMode.SafeArea:
      case ClusterMode.Island:
      default:
        return 'blue'
    }
  }

  protected async load(): Promise<void> {
    this.isLoading = true
    try {
      const responseCluster = await this.$axios.get<ClusterResultModel>(`/api/cluster`)
      this.clusterData = responseCluster.data
      this.isLoading = false
    } catch (e) {
      console.log(e)
    }
  }
}
</script>

<style lang="scss">
</style>
