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
        <template v-if="isAuthenticated">
          <v-list-item class="px-2">
            <v-list-item-avatar>
              <v-img :src="discordAvatarUrl"></v-img>
            </v-list-item-avatar>
          </v-list-item>
          <v-divider></v-divider>
        </template>
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
    </v-app-bar>
    <v-main class="main">
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts">
import './assets/custom.scss'
import { Action, Getter } from 'vuex-class'
import { Component, Vue } from 'vue-property-decorator'
const namespace = 'account'

@Component({
  components: {}
})
export default class App extends Vue {
  private clipped = true
  private drawer = true
  private miniVariant = true
  private right = true
  private title = 'Albion Arms'
  private items = [
    { title: 'Home', icon: 'mdi-home', link: '/' },
    { title: 'Battles', icon: 'mdi-sword-cross', link: '/battles' },
    {
      title: 'Discord',
      icon: 'mdi-discord',
      link: 'https://discord.gg/d3YpWbBX7u',
      target: '_blank'
    },
    {
      title: 'PayPal',
      icon: 'mdi-currency-usd',
      link: 'https://www.paypal.com/donate?hosted_button_id=UBH8WVHEBS2SG',
      target: '_blank'
    }
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
    this.load()
  }

  protected load(): void {
    fetch(`/authentication/data`)
      .then((res) => res.clone().json())
      .then((res) => {
        this.setAuthenticated(res.isAuthenticated)
        this.setDiscordId(res.discordId)
        this.setDiscordAvatarUrl(res.discordAvatarUrl)
        console.log(this.discordId)
      })
      .catch((err) => {
        console.log(err)
      })
  }
}
</script>

<style lang="scss">
</style>
