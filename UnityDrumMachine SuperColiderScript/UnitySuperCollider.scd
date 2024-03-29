

( //set up OSC reciever
	n = NetAddr("127.0.0.1", NetAddr.langPort); // local machine

	OSCdef(\red, {|msg, time, addr, recvPort| Synth(\kick)}, '/red');

    OSCdef(\orange, {|msg, time, addr, recvPort| Synth(\neurosnare)}, '/orange');

	OSCdef(\yellow, {|msg, time, addr, recvPort| Synth(\rhodey_sc)}, '/yellow');

    //OSCdef(\OverloadSound, {|msg, time, addr, recvPort| Synth(\overload)}, '/overload');

	//OSCdef(\green, {|msg, time, addr, recvPort| )}, '/green');
	//OSCdef(\purple, {|msg, time, addr, recvPort| }, '/purple');
)

{ //Test OSC reciever
	m = NetAddr("127.0.0.1", NetAddr.langPort); // loopback
	m.sendMsg("/red", 0.5);
	m.sendMsg("/orange", 0.5);
	m.sendMsg("/yellow", 0.5);
	//m.sendMsg("/green", 0.5);
	//m.sendMsg("/purple", 0.5);
)

//Setup isntuments
(
SynthDef(\rhodey_sc, {
    |
    // standard meanings
    out = 0, freq = 55, gate = 1, pan = 0.5, amp = 0.1,
    // all of these range from 0 to 1
    vel = 0.8, modIndex = 0.5, mix = 0.2, lfoSpeed = 3, lfoDepth = 0.2
    |
    var env1, env2, env3, env4;
    var osc1, osc2, osc3, osc4, snd;

    lfoSpeed = lfoSpeed * 12;

    freq = freq * 2;

    env1 = EnvGen.ar(Env.adsr(0.001, 0.50, 0.0, 0.04, curve: \lin));
    env2 = EnvGen.ar(Env.adsr(0.001, 0.25, 0.0, 0.04, curve: \lin));
    env3 = EnvGen.ar(Env.adsr(0.001, 0.6, 0.0, 0.04, curve: \lin));
    env4 = EnvGen.ar(Env.adsr(0.001, 0.6, 0.0, 0.04, curve: \lin));

    osc4 = SinOsc.ar(freq * 0.5) * 2pi * 2 * 0.535887 * modIndex * env4 * vel;
    osc3 = SinOsc.ar(freq, osc4) * env3 * vel;
    osc2 = SinOsc.ar(freq * 15) * 2pi * 0.108819 * env2 * vel;
    osc1 = SinOsc.ar(freq, osc2) * env1 * vel;
    snd = Mix((osc3 * (1 - mix)) + (osc1 * mix));
    snd = snd * (SinOsc.ar(lfoSpeed) * lfoDepth + 1);

    // using the doneAction: 2 on the other envs can create clicks (bc of the linear curve maybe?)
    snd = snd * EnvGen.ar(Env.asr(0, 1, 0.1), gate, doneAction:Done.freeSelf);
    snd = Pan2.ar(snd, pan, amp);

    Out.ar(out, snd);
}).add;
)

/*
(
e.stream = Pbind(
    \scale, Scale.mixolydian,
    \octave, 4,
	\root, [2],
    \degree, Prand(#[0, 2, 3, 5, 0, 2, 3, 5, 7, 9, 10, 15, 10, 9, 7, 7, 4, 2
    ], inf),
).asStream();
)
*/

(
SynthDef(\neurosnare, {
    var snd;
    // a percussive click to give it some attack
    snd = LPF.ar(HPF.ar(WhiteNoise.ar, 300), 8000) * Env.linen(0.001, 0.01, 0.001).ar;
    // sine sweep body. very important!
    snd = snd + (SinOsc.ar(Env([400, 196, 160], [0.04, 0.2], \exp).ar) * Env.perc(0.04, 0.2).ar * 6.dbamp).tanh;
    // sound of snare coils rattling
    snd = snd + (HPF.ar(BPeakEQ.ar(WhiteNoise.ar, 4000, 0.5, 3), 300) * Env.perc(0.05, 0.2).delay(0.01).ar(2) * -3.dbamp);
    // another sound sweep to improve the attack, optional
    snd = snd + (SinOsc.ar(XLine.kr(3000, 1500, 0.01)) * Env.perc(0.001, 0.02).ar);
    // distortion helps glue everything together and acts as a compressor
    snd = (snd * 1.4).tanh;
    snd = Pan2.ar(snd, \pan.kr(0), \amp.kr(0.1));
    Out.ar(\out.kr(0), snd);
}).add;
)

(
SynthDef(\kick, {
    var snd;
    snd = DC.ar(0);
    snd = snd + (SinOsc.ar(XLine.ar(1500, 800, 0.01)) * Env.perc(0.0005, 0.01, curve: \lin).ar);
    snd = snd + (BPF.ar(Impulse.ar(0) * SampleRate.ir / 48000, 6100, 1.0) * 3.dbamp);
    snd = snd + (BPF.ar(Hasher.ar(Sweep.ar), 300, 0.9) * Env.perc(0.001, 0.02).ar);
    snd = snd + (SinOsc.ar(XLine.ar(472, 60, 0.045)) * Env.perc(0.0001, 0.3, curve: \lin).delay(0.005).ar(2));
    snd = snd.tanh;
    Out.ar(\out.kr(0), Pan2.ar(snd, \pan.kr(0), \amp.kr(0.1)));
}).add;
)




