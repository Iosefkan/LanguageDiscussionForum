<script context="module">
	let current;
</script>

<script>
	export let src;
	export let title;

	let time = 0;
	let duration = 0;
	let paused = true;

	function format(time) {
		if (isNaN(time)) return '...';

		const minutes = Math.floor(time / 60);
		const seconds = Math.floor(time % 60);

		return `${minutes}:${seconds < 10 ? `0${seconds}` : seconds}`;
	}
</script>

<div class="player mb-2 {paused ? "" : "shadow dark:shadow-gray-400 shadow-black"}" class:paused>
	<audio
		src={window.location.origin.concat("/fallback/audio/").concat(src)}
		bind:currentTime={time}
		bind:duration
		bind:paused
		on:play={(e) => {
			const audio = e.currentTarget;

			if (audio !== current) {
				current?.pause();
				current = audio;
			}
		}}
		on:ended={() => {
			time = 0;
		}}
	/>

	<button
		class="play hover:bg-blue-600 bg-blue-500"
		aria-label={paused ? 'play' : 'pause'}
		on:click={() => paused = !paused}
	/>

	<div class=" text-gray-700 dark:text-gray-400 overflow-hidden">
		<div class="description">
			{title}
		</div>

		<div class="time text-gray-700 dark:text-gray-400 flex items-center gap-2">
			<span>{format(time)}</span>
			<div
				class="slider bg-gray-700 dark:bg-gray-400"
				on:pointerdown={e => {
					const div = e.currentTarget;

					function seek(e) {
						const { left, width } = div.getBoundingClientRect();

						let p = (e.clientX - left) / width;
						if (p < 0) p = 0;
						if (p > 1) p = 1;

						time = p * duration;
					}

					seek(e);

					window.addEventListener('pointermove', seek);

					window.addEventListener('pointerup', () => {
						window.removeEventListener('pointermove', seek);
					}, {
						once: true
					});
				}}
			>
				<div class="progress dark:bg-gray-700 bg-gray-400" style="--progress: {time / duration}%" />
			</div>
			<span>{duration ? format(duration) : '--:--'}</span>
		</div>
	</div>
</div>

<style>
	.player {
		display: grid;
		grid-template-columns: 2.5em 1fr;
		align-items: center;
		gap: 1em;
		padding: 0.5em 1em 0.5em 0.5em;
		border-radius: 2em;
		background: var(--bg-1);
		transition: filter 0.2s;
		color: var(--fg-3);
		user-select: none;
	}

	button {
		width: 100%;
		aspect-ratio: 1;
		background-repeat: no-repeat;
		background-position: 50% 50%;
		border-radius: 50%;
	}

	[aria-label="pause"] {
		background-image: url(../assets/pause.svg);
	}

	[aria-label="play"] {
		background-image: url(../assets/play.svg);
	}

	.description {
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		line-height: 1.2;
	}

	.time span {
		font-size: 0.7em;
	}

	.slider {
		flex: 1;
		height: 0.5em;
		border-radius: 0.5em;
		overflow: hidden;
	}

	.progress {
		width: calc(100 * var(--progress));
		height: 100%;
	}
</style>