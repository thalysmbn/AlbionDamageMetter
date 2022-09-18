<template>
  <v-data-table
    :headers="dessertHeaders"
    :items="events"
    item-key="eventId"
    hide-default-header
    :loading="loading"
    :item-class="itemRowBackground"
    :footer-props="{
      'items-per-page-options': [10, 20, 30, 40, 50]
    }"
    class="killBoard"
  >
    <template v-slot:item.index="{ item }" class="label">
      <div
        :class="
          'text ' +
          (item.killer.name == id || item.killer.guildName == id
            ? 'kill'
            : item.victim.name == id || item.victim.guildName == id
            ? 'death'
            : 'assist')
        "
      >
        <template v-if="item.killer.name == id || item.killer.guildName == id">KILL</template>
        <template v-else-if="item.victim.name == id || item.victim.guildName == id">DEATH</template>
        <template v-else>ASSIST</template>
      </div>
    </template>
    <template v-slot:item.me="{ item }">
      <template v-if="item.killer.name == id || item.killer.guildName == id">
        <span class="nick transition-swing text-subtitle-2">{{ item.killer.name }}</span
        ><template v-if="item.killer.allianceName != ''">[{{ item.killer.allianceName }}] </template
        >{{ item.killer.guildName }}
      </template>
      <template v-else-if="item.victim.name == id || item.victim.guildName == id">
        <span class="nick transition-swing text-subtitle-2">{{ item.victim.name }}</span
        ><template v-if="item.victim.allianceName != ''">[{{ item.victim.allianceName }}] </template
        >{{ item.victim.guildName }}
      </template>
      <template v-else-if="assist">
        <span class="nick transition-swing text-subtitle-2">{{ assist.name }}</span
        ><template v-if="assist.allianceName != ''">[{{ assist.allianceName }}] </template
        >{{ assist.guildName }}
      </template>
    </template>
    <template v-slot:item.fame="{ item }">
      <div class="transition-swing text-subtitle-2">
        {{ item.totalVictimKillFame | numFormatter }} Fame
      </div>
      <div class="transition-swing text-caption">
        {{ item.timeStamp | formatDate }}
      </div>
    </template>
    <template v-slot:item.other="{ item }">
      <template v-if="item.killer.name == id || item.killer.guildName == id">
        <span class="nick transition-swing text-subtitle-2">{{ item.victim.name }}</span
        ><template v-if="item.victim.allianceName != ''">[{{ item.victim.allianceName }}] </template
        >{{ item.victim.guildName }}
      </template>
      <template v-else-if="item.victim.name == id || item.victim.guildName == id">
        <span class="nick transition-swing text-subtitle-2">{{ item.killer.name }}</span
        ><template v-if="item.killer.allianceName != ''">[{{ item.killer.allianceName }}] </template
        >{{ item.killer.guildName }}
      </template>
      <template v-else>
        <span class="nick transition-swing text-subtitle-2">{{ item.victim.name }}</span
        ><template v-if="item.victim.allianceName != ''">[{{ item.victim.allianceName }}] </template
        >{{ item.victim.guildName }}
      </template>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import { EventModel } from '@/models/EventModel'
import { PlayerModel } from '@/models/PlayerModel'
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'app-data-table'
})
export default class VDataTable extends Vue {
  @Prop({ default: '' }) public id!: string
  @Prop({ default: null }) public events!: Array<EventModel>
  @Prop({ default: true }) public loading!: boolean
  @Prop({ default: null }) public assist!: PlayerModel

  protected dessertHeaders = [
    { text: '', value: 'index', tdClass: 'label' },
    { text: '', value: 'me' },
    { text: '', value: 'fame', align: 'center' },
    { text: '', value: 'other' }
  ]

  protected itemRowBackground(item: EventModel): string {
    return (
      'event ' +
      (item.killer.name == this.id ? 'kill' : item.victim.name == this.id ? 'death' : 'assist')
    )
  }
}
</script>

<style lang="scss">
.killBoard {
  .nick {
    padding-right: 5px;
  }
}
</style>
